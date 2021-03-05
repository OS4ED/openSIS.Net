using opensis.data.Helper;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.Notice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opensis.data.Repository
{
    public class NoticeRepository : INoticeRepository
    {
        private CRMContext context;
        private static readonly string NORECORDFOUND = "No Record Found";
        public NoticeRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }

        /// <summary>
        /// Add Notice
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public NoticeAddViewModel AddNotice(NoticeAddViewModel notice)
        {
            try
            {
                //int? noticeId = Utility.GetMaxPK(this.context, new Func<Notice, int>(x => x.NoticeId));

                int? noticeId = 1;

                var NoticeData = this.context?.Notice.Where(x => x.SchoolId == notice.Notice.SchoolId && x.TenantId == notice.Notice.TenantId).OrderByDescending(x => x.NoticeId).FirstOrDefault();

                if (NoticeData != null)
                {
                    noticeId = NoticeData.NoticeId + 1;
                }

                notice.Notice.NoticeId = (int)noticeId;
                notice.Notice.TenantId = notice.Notice.TenantId;
                notice.Notice.Isactive = true;
                notice.Notice.ValidFrom = notice.Notice.ValidFrom;
                notice.Notice.ValidTo = notice.Notice.ValidTo;
                notice.Notice.CreatedTime = DateTime.UtcNow;
                this.context?.Notice.Add(notice.Notice);
                this.context?.SaveChanges();
                notice._failure = false;
                notice._message = "Notice Added Successfuly";
                return notice;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// Get Notice By Id
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public NoticeAddViewModel ViewNotice(NoticeAddViewModel notice)
        {
            try
            {
                NoticeAddViewModel noticeAddViewModel = new NoticeAddViewModel();
                var noticeModel = this.context?.Notice.FirstOrDefault(x => x.TenantId == notice.Notice.TenantId && x.SchoolId == notice.Notice.SchoolId && x.NoticeId == notice.Notice.NoticeId);
                if (noticeModel != null)
                {
                    noticeAddViewModel.Notice = noticeModel;
                }
                else
                {
                    noticeAddViewModel._failure = true;
                    noticeAddViewModel._message = NORECORDFOUND;
                    return noticeAddViewModel;
                }

                return noticeAddViewModel;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// Delete Notice
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public NoticeDeleteModel DeleteNotice(NoticeDeleteModel notice)
        {
            try
            {
                var noticeRepository = this.context?.Notice.FirstOrDefault(x => x.TenantId == notice.TenantId && x.SchoolId == notice.SchoolId && x.NoticeId == notice.NoticeId);

                noticeRepository.Isactive = false;
                this.context?.SaveChanges();
                notice._failure = false;
                notice._message = "Notice Deleted Successfuly";
                return notice;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// Update Notice
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public NoticeAddViewModel UpdateNotice(NoticeAddViewModel notice)
        {
            try
            {
                var noticeRepository = this.context?.Notice.FirstOrDefault(x => x.TenantId == notice.Notice.TenantId && x.SchoolId == notice.Notice.SchoolId && x.NoticeId == notice.Notice.NoticeId);

                if (noticeRepository != null)
                {
                    notice.Notice.Isactive = true;
                    notice.Notice.CreatedBy = noticeRepository.CreatedBy;
                    notice.Notice.CreatedTime = noticeRepository.CreatedTime;
                    this.context.Entry(noticeRepository).CurrentValues.SetValues(notice.Notice);
                    this.context?.SaveChanges();
                    notice._failure = false;
                    notice._message = "Notice Updated Successfuly";
                    return notice;
                }
                else
                {
                    notice = null;
                    notice._failure = true;
                    notice._message = NORECORDFOUND;
                    return notice;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// Get all Notice
        /// </summary>
        /// <returns></returns>
        public NoticeListViewModel GetAllNotice(NoticeListViewModel noticeList)
        {
            NoticeListViewModel getAllNoticeList = new NoticeListViewModel();
            try
            {
                var noticeRepository = this.context?.Notice.OrderBy(x => x.ValidFrom).Where(x => x.TenantId == noticeList.TenantId && x.SchoolId == noticeList.SchoolId && x.Isactive == true).ToList();
                if (noticeRepository.Count > 0)
                {
                    foreach (var notice in noticeRepository)
                    {
                        if (!string.IsNullOrEmpty(notice.TargetMembershipIds))
                        {
                            string[] membersList = notice.TargetMembershipIds.Split(",");
                            int[] memberIds = Array.ConvertAll(membersList, s => int.Parse(s));
                            var profiles = this.context?.Membership.Where(t => memberIds.Contains(t.MembershipId) && t.SchoolId == noticeList.SchoolId).Select(t => t.Profile).ToArray();
                            var mebershipIds = string.Join(",", profiles);
                            notice.TargetMembershipIds = mebershipIds;
                        }
                    }

                    getAllNoticeList.NoticeList = noticeRepository;
                    getAllNoticeList._failure = false;
                    return getAllNoticeList;
                }
                else
                {
                    getAllNoticeList._failure = true;
                    getAllNoticeList._message = NORECORDFOUND;
                    return getAllNoticeList;
                }

            }
            catch (Exception ex)
            {
                getAllNoticeList.NoticeList = null;
                getAllNoticeList._failure = true;
                getAllNoticeList._message = ex.Message;
                return getAllNoticeList;
            }
        }
    }
}
