using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiffingAPITask.BusinessLogic.DTO;
using DiffingAPITask.BusinessLogic.Services.Interfaces;
using DiffingAPITask.Data.Entities;
using DiffingAPITask.Data.Repositories.Interfaces;

namespace DiffingAPITask.BusinessLogic.Services
{
    public class DiffService : IDiffService
    {
        private readonly IDataItemRepository _itemRepository;
        private readonly IDataValidationService _validationService;
        
        public DiffService(IDataItemRepository itemRepository, IDataValidationService validationService)
        {
            _itemRepository = itemRepository;
            _validationService = validationService;
        }
        
        public async Task SetRight(int id, string data)
        {
            if (!_validationService.IsBase64String(data))
            {
                throw new ArgumentException("Data must be a valid base64 string", nameof(data));
            }
            
            if (!await _itemRepository.Exists(id))
            {
                var newItem = new DataItem
                {
                    Id = id,
                    Left = null,
                    Right = data
                };

                await _itemRepository.AddAsync(newItem);
            }
            else
            {
                var existedItem = await _itemRepository.FindAsync(id);
                existedItem.Right = data;
                await _itemRepository.UpdateAsync(existedItem);
            }
        }

        public async Task SetLeft(int id, string data)
        {
            if (!_validationService.IsBase64String(data))
            {
                throw new ArgumentException("Data must be a valid base64 string", nameof(data));
            }
            
            if (!await _itemRepository.Exists(id))
            {
                var newItem = new DataItem
                {
                    Id = id,
                    Left = data,
                    Right = null
                };

                await _itemRepository.AddAsync(newItem);
            }
            else
            {
                var existedItem = await _itemRepository.FindAsync(id);
                existedItem.Left = data;
                await _itemRepository.UpdateAsync(existedItem);
            }
        }

        public async Task<DiffResultDTO> GetDiffResult(int id)
        {
            var item = await _itemRepository.FindAsync(id);
            
            if (item is null || !item.IsReady())
            {
                return null;
            }

            return Diff(item);
        }

        private DiffResultDTO Diff(DataItem item)
        {
            byte[] right = Convert.FromBase64String(item.Right);
            byte[] left = Convert.FromBase64String(item.Left);
            
            if (right.Length != left.Length)
            {
                return new DiffResultDTO
                {
                    DiffResultType = DiffResultDTO.ResultType.SizeDoNotMatch
                };
            }

            var diffs = new List<object>();

            var diffOffset = -1;
            var diffLength = 0;
            
            var inDiffSegment = false;
            
            for (int i = 0; i < right.Length; i++)
            {
                if (right[i] != left[i])
                {
                    if (!inDiffSegment)
                    {
                        inDiffSegment = true;
                        diffOffset = i;
                    }
                    
                    diffLength += 1;
                }
                else if (inDiffSegment)
                {
                    diffs.Add(new {offset = diffOffset, length = diffLength});
                    inDiffSegment = false;
                    diffLength = 0;
                }

                if (inDiffSegment && i == right.Length - 1)
                {
                    diffs.Add(new {offset = diffOffset, length = diffLength});
                }
            }

            if (diffs.Count == 0)
            {
                return new DiffResultDTO
                {
                    DiffResultType = DiffResultDTO.ResultType.Equals
                };   
            }

            var result = new DiffResultDTO
            {
                DiffResultType = DiffResultDTO.ResultType.ContentDoNotMatch
            };

            result.Extensions.Add("diffs", diffs);

            return result;

        }
    }
}