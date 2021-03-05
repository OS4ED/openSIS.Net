using Microsoft.EntityFrameworkCore;
using opensis.data.Helper;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.Period;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opensis.data.Repository
{
    public class PeriodRepository: IPeriodRepository
    {
        private CRMContext context;
        private static readonly string NORECORDFOUND = "No Record Found";
        public PeriodRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }

        /// <summary>
        /// Add Block
        /// </summary>
        /// <param name="blockAddViewModel"></param>
        /// <returns></returns>
        public BlockAddViewModel AddBlock(BlockAddViewModel blockAddViewModel)
        {
            try
            {
                var blockTitle = this.context?.Block.FirstOrDefault(x => x.TenantId == blockAddViewModel.block.TenantId && x.SchoolId == blockAddViewModel.block.SchoolId && x.BlockTitle.ToLower() == blockAddViewModel.block.BlockTitle.ToLower());

                if (blockTitle == null)
                {
                    int? BlockId = 1;

                    var blockData = this.context?.Block.Where(x => x.SchoolId == blockAddViewModel.block.SchoolId && x.TenantId == blockAddViewModel.block.TenantId).OrderByDescending(x => x.BlockId).FirstOrDefault();

                    if (blockData != null)
                    {
                        BlockId = blockData.BlockId + 1;
                    }

                    blockAddViewModel.block.BlockId = (int)BlockId;
                    blockAddViewModel.block.CreatedOn = DateTime.UtcNow;
                    this.context?.Block.Add(blockAddViewModel.block);
                    this.context?.SaveChanges();
                    blockAddViewModel._failure = false;
                    blockAddViewModel._message = "Block Added Successfully";
                }
                else
                {
                    blockAddViewModel._failure = true;
                    blockAddViewModel._message = "Block Title Already Exists";
                }
            }
            catch (Exception es)
            {
                blockAddViewModel._failure = true;
                blockAddViewModel._message = es.Message;
            }
            return blockAddViewModel;
        }

        /// <summary>
        /// Update Block
        /// </summary>
        /// <param name="blockAddViewModel"></param>
        /// <returns></returns>
        public BlockAddViewModel UpdateBlock(BlockAddViewModel blockAddViewModel)
        {
            try
            {
                var blockUpdate = this.context?.Block.FirstOrDefault(x => x.TenantId == blockAddViewModel.block.TenantId && x.SchoolId == blockAddViewModel.block.SchoolId && x.BlockId == blockAddViewModel.block.BlockId);

                if (blockUpdate != null)
                {
                    var blockTitle = this.context?.Block.FirstOrDefault(x => x.TenantId == blockAddViewModel.block.TenantId && x.SchoolId == blockAddViewModel.block.SchoolId && x.BlockTitle.ToLower() == blockAddViewModel.block.BlockTitle.ToLower() && x.BlockId != blockAddViewModel.block.BlockId);

                    if (blockTitle == null)
                    {
                        blockAddViewModel.block.CreatedBy = blockUpdate.CreatedBy;
                        blockAddViewModel.block.CreatedOn = blockUpdate.CreatedOn;
                        blockAddViewModel.block.UpdatedOn = DateTime.Now;
                        this.context.Entry(blockUpdate).CurrentValues.SetValues(blockAddViewModel.block);
                        this.context?.SaveChanges();
                        blockAddViewModel._failure = false;
                        blockAddViewModel._message = "Block Updated Successfully";
                    }
                    else
                    {
                        blockAddViewModel._failure = true;
                        blockAddViewModel._message = "Block Title Already Exists";
                    }
                }
                else
                {
                    blockAddViewModel._failure = true;
                    blockAddViewModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                blockAddViewModel._failure = true;
                blockAddViewModel._message = es.Message;
            }
            return blockAddViewModel;
        }

        /// <summary>
        /// Delete Block
        /// </summary>
        /// <param name="blockAddViewModel"></param>
        /// <returns></returns>
        public BlockAddViewModel DeleteBlock(BlockAddViewModel blockAddViewModel)
        {
            try
            {
                var blockDelete = this.context?.Block.FirstOrDefault(x => x.TenantId == blockAddViewModel.block.TenantId && x.SchoolId == blockAddViewModel.block.SchoolId && x.BlockId == blockAddViewModel.block.BlockId);

                if (blockDelete != null)
                {
                    var blockPeriodExits = this.context?.BlockPeriod.FirstOrDefault(x => x.TenantId == blockDelete.TenantId && x.SchoolId == blockDelete.SchoolId && x.BlockId == blockDelete.BlockId);
                    if (blockPeriodExits != null)
                    {
                        blockAddViewModel._failure = true;
                        blockAddViewModel._message = "Cannot delete because it has association.";
                    }
                    else
                    {
                        this.context?.Block.Remove(blockDelete);
                        this.context?.SaveChanges();
                        blockAddViewModel._failure = false;
                        blockAddViewModel._message = "Block Deleted Successfully";
                    }
                }
                else
                {
                    blockAddViewModel._failure = true;
                    blockAddViewModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                blockAddViewModel._failure = true;
                blockAddViewModel._message = es.Message;
            }
            return blockAddViewModel;
        }

        /// <summary>
        /// Add Block Period
        /// </summary>
        /// <param name="blockPeriodAddViewModel"></param>
        /// <returns></returns>
        public BlockPeriodAddViewModel AddBlockPeriod(BlockPeriodAddViewModel blockPeriodAddViewModel)
        {
            try
            {
                var periodTitle = this.context?.BlockPeriod.FirstOrDefault(x => x.TenantId == blockPeriodAddViewModel.blockPeriod.TenantId && x.SchoolId == blockPeriodAddViewModel.blockPeriod.SchoolId && x.PeriodTitle.ToLower() == blockPeriodAddViewModel.blockPeriod.PeriodTitle.ToLower() && x.BlockId == blockPeriodAddViewModel.blockPeriod.BlockId);

                if (periodTitle == null)
                {
                    int? PeriodId = 1;
                    int? SortOrder = 1;

                    var blockPeriodData = this.context?.BlockPeriod.Where(x => x.TenantId == blockPeriodAddViewModel.blockPeriod.TenantId && x.SchoolId == blockPeriodAddViewModel.blockPeriod.SchoolId).OrderByDescending(x => x.PeriodId).FirstOrDefault();

                    if (blockPeriodData != null)
                    {
                        PeriodId = blockPeriodData.PeriodId + 1;
                    }

                    var sortOrderData = this.context?.BlockPeriod.Where(x => x.TenantId == blockPeriodAddViewModel.blockPeriod.TenantId && x.SchoolId == blockPeriodAddViewModel.blockPeriod.SchoolId && x.BlockId == blockPeriodAddViewModel.blockPeriod.BlockId).OrderByDescending(x => x.PeriodSortOrder).FirstOrDefault();

                    if (sortOrderData != null)
                    {
                        SortOrder = sortOrderData.PeriodSortOrder + 1;
                    }
                    blockPeriodAddViewModel.blockPeriod.PeriodId = (int)PeriodId;
                    blockPeriodAddViewModel.blockPeriod.PeriodSortOrder = (int)SortOrder;
                    blockPeriodAddViewModel.blockPeriod.CreatedOn = DateTime.UtcNow;
                    this.context?.BlockPeriod.Add(blockPeriodAddViewModel.blockPeriod);
                    this.context?.SaveChanges();
                    blockPeriodAddViewModel._failure = false;
                    blockPeriodAddViewModel._message = "Period Added Successfully";
                }
                else
                {
                    blockPeriodAddViewModel._failure = true;
                    blockPeriodAddViewModel._message = "Period Title Already Exists";
                }
            }
            catch (Exception es)
            {
                blockPeriodAddViewModel._failure = true;
                blockPeriodAddViewModel._message = es.Message;
            }
            return blockPeriodAddViewModel;
        }

        /// <summary>
        /// Update BlockPeriod
        /// </summary>
        /// <param name="blockPeriodAddViewModel"></param>
        /// <returns></returns>
        public BlockPeriodAddViewModel UpdateBlockPeriod(BlockPeriodAddViewModel blockPeriodAddViewModel)
        {
            try
            {
                var blockPeriodUpdate = this.context?.BlockPeriod.FirstOrDefault(x => x.TenantId == blockPeriodAddViewModel.blockPeriod.TenantId && x.SchoolId == blockPeriodAddViewModel.blockPeriod.SchoolId && x.PeriodId == blockPeriodAddViewModel.blockPeriod.PeriodId);

                if (blockPeriodUpdate != null)
                {
                    var periodTitle = this.context?.BlockPeriod.FirstOrDefault(x => x.TenantId == blockPeriodAddViewModel.blockPeriod.TenantId && x.SchoolId == blockPeriodAddViewModel.blockPeriod.SchoolId && x.BlockId == blockPeriodAddViewModel.blockPeriod.BlockId && x.PeriodId != blockPeriodAddViewModel.blockPeriod.PeriodId && x.PeriodTitle.ToLower() == blockPeriodAddViewModel.blockPeriod.PeriodTitle.ToLower());

                    if (periodTitle == null)
                    {
                        blockPeriodAddViewModel.blockPeriod.CreatedBy = blockPeriodUpdate.CreatedBy;
                        blockPeriodAddViewModel.blockPeriod.CreatedOn = blockPeriodUpdate.CreatedOn;
                        blockPeriodAddViewModel.blockPeriod.PeriodSortOrder = blockPeriodUpdate.PeriodSortOrder;
                        blockPeriodAddViewModel.blockPeriod.UpdatedOn = DateTime.Now;
                        this.context.Entry(blockPeriodUpdate).CurrentValues.SetValues(blockPeriodAddViewModel.blockPeriod);
                        this.context?.SaveChanges();
                        blockPeriodAddViewModel._failure = false;
                        blockPeriodAddViewModel._message = "Period Updated Successfully";                        
                    }
                    else
                    {
                        blockPeriodAddViewModel._failure = true;
                        blockPeriodAddViewModel._message = "Period Title Already Exists";
                    }
                }
                else
                {
                    blockPeriodAddViewModel._failure = true;
                    blockPeriodAddViewModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                blockPeriodAddViewModel._failure = true;
                blockPeriodAddViewModel._message = es.Message;
            }
            return blockPeriodAddViewModel;
        }

        /// <summary>
        /// Delete BlockPeriod
        /// </summary>
        /// <param name="blockPeriodAddViewModel"></param>
        /// <returns></returns>
        public BlockPeriodAddViewModel DeleteBlockPeriod(BlockPeriodAddViewModel blockPeriodAddViewModel)
        {
            try
            {
                var blockPeriodDelete = this.context?.BlockPeriod.FirstOrDefault(x => x.TenantId == blockPeriodAddViewModel.blockPeriod.TenantId && x.SchoolId == blockPeriodAddViewModel.blockPeriod.SchoolId && x.PeriodId == blockPeriodAddViewModel.blockPeriod.PeriodId);

                if (blockPeriodDelete != null)
                {
                    this.context?.BlockPeriod.Remove(blockPeriodDelete);
                    this.context?.SaveChanges();
                    blockPeriodAddViewModel._failure = false;
                    blockPeriodAddViewModel._message = "Period Deleted Successfully";
                }
                else
                {
                    blockPeriodAddViewModel._failure = true;
                    blockPeriodAddViewModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                blockPeriodAddViewModel._failure = true;
                blockPeriodAddViewModel._message = es.Message;
            }
            return blockPeriodAddViewModel;
        }

        /// <summary>
        /// Get All BlockList
        /// </summary>
        /// <param name="blockListViewModel"></param>
        /// <returns></returns>
        public BlockListViewModel GetAllBlockList(BlockListViewModel blockListViewModel)
        {
            BlockListViewModel blockListModel = new BlockListViewModel();
            try
            {
                var blockDataList = this.context?.Block.Where(x => x.TenantId == blockListViewModel.TenantId && x.SchoolId == blockListViewModel.SchoolId).OrderBy(x => x.BlockSortOrder).ToList();
                
                if (blockDataList.Count > 0)
                {
                    foreach (var block in blockDataList)
                    {
                        var blockList = new GetBlockListForView()
                        {
                            TenantId = block.TenantId,
                            SchoolId = block.SchoolId,
                            BlockId = block.BlockId,
                            BlockTitle = block.BlockTitle,
                            BlockSortOrder = block.BlockSortOrder,
                            CreatedBy = block.CreatedBy,
                            CreatedOn = block.CreatedOn,
                            UpdatedBy = block.UpdatedBy,
                            UpdatedOn = block.UpdatedOn
                        };              
                        var blockPeriodDataList = this.context?.BlockPeriod.Where(x => x.TenantId == block.TenantId && x.SchoolId == block.SchoolId && x.BlockId==block.BlockId).OrderBy(x => x.PeriodSortOrder).ToList();
                        if(blockPeriodDataList.Count>0)
                        {
                            blockList.BlockPeriod = blockPeriodDataList;
                        }
                        //block.BlockPeriod = block.BlockPeriod.OrderBy(x => x.PeriodSortOrder).ToList();
                        blockListModel.getBlockListForView.Add(blockList);
                    }
                    blockListModel._failure = false;
                }
                else
                {
                    blockListModel._message = NORECORDFOUND;
                    blockListModel._failure = true;
                }

                blockListModel.TenantId = blockListViewModel.TenantId;
                blockListModel.SchoolId = blockListViewModel.SchoolId;
                blockListModel._tenantName = blockListViewModel._tenantName;
                blockListModel._token = blockListViewModel._token;

            }
            catch (Exception es)
            {
                blockListModel.getBlockListForView = null;
                blockListModel._message = es.Message;
                blockListModel._failure = true;
                blockListModel._tenantName = blockListViewModel._tenantName;
                blockListModel._token = blockListViewModel._token;
            }
            return blockListModel;
        }

        /// <summary>
        /// Update BlockPeriod SortOrder
        /// </summary>
        /// <param name="blockPeriodSortOrderViewModel"></param>
        /// <returns></returns>
        public BlockPeriodSortOrderViewModel UpdateBlockPeriodSortOrder(BlockPeriodSortOrderViewModel blockPeriodSortOrderViewModel)
        {
            try
            {
                var blockPeriodRecords = new List<BlockPeriod>();

                var targetBlockPeriod = this.context?.BlockPeriod.FirstOrDefault(x => x.PeriodSortOrder == blockPeriodSortOrderViewModel.PreviousSortOrder && x.SchoolId == blockPeriodSortOrderViewModel.SchoolId && x.TenantId == blockPeriodSortOrderViewModel.TenantId && x.BlockId == blockPeriodSortOrderViewModel.BlockId);
                targetBlockPeriod.PeriodSortOrder = blockPeriodSortOrderViewModel.CurrentSortOrder;

                if (blockPeriodSortOrderViewModel.PreviousSortOrder > blockPeriodSortOrderViewModel.CurrentSortOrder)
                {
                    blockPeriodRecords = this.context?.BlockPeriod.Where(x => x.PeriodSortOrder >= blockPeriodSortOrderViewModel.CurrentSortOrder && x.PeriodSortOrder < blockPeriodSortOrderViewModel.PreviousSortOrder && x.TenantId == blockPeriodSortOrderViewModel.TenantId && x.SchoolId == blockPeriodSortOrderViewModel.SchoolId && x.BlockId == blockPeriodSortOrderViewModel.BlockId).ToList();

                    if (blockPeriodRecords.Count > 0)
                    {
                        blockPeriodRecords.ForEach(x => x.PeriodSortOrder = x.PeriodSortOrder + 1);
                    }
                }
                if (blockPeriodSortOrderViewModel.CurrentSortOrder > blockPeriodSortOrderViewModel.PreviousSortOrder)
                {
                    blockPeriodRecords = this.context?.BlockPeriod.Where(x => x.PeriodSortOrder <= blockPeriodSortOrderViewModel.CurrentSortOrder && x.PeriodSortOrder > blockPeriodSortOrderViewModel.PreviousSortOrder && x.SchoolId == blockPeriodSortOrderViewModel.SchoolId && x.TenantId == blockPeriodSortOrderViewModel.TenantId && x.BlockId == blockPeriodSortOrderViewModel.BlockId).ToList();
                    if (blockPeriodRecords.Count > 0)
                    {
                        blockPeriodRecords.ForEach(x => x.PeriodSortOrder = x.PeriodSortOrder - 1);
                    }
                }
                this.context?.SaveChanges();
                blockPeriodSortOrderViewModel._failure = false;
            }
            catch (Exception es)
            {
                blockPeriodSortOrderViewModel._message = es.Message;
                blockPeriodSortOrderViewModel._failure = true;
            }
            return blockPeriodSortOrderViewModel;
        }
    }
}
