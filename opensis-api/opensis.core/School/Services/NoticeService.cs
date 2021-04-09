using opensis.core.helper;
using opensis.core.School.Interfaces;
using opensis.data.Interface;
using opensis.data.ViewModels.Notice;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.School.Services
{
    public class NoticeService : INoticeService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";
        public INoticeRepository noticeRepository;
        public NoticeService(INoticeRepository noticeRepository)
        {
            this.noticeRepository = noticeRepository;
        }


        public NoticeAddViewModel SaveNotice(NoticeAddViewModel notice)
        {
            NoticeAddViewModel noticeAddViewModel = new NoticeAddViewModel();
            if (TokenManager.CheckToken(notice._tenantName + notice._userName, notice._token))
            {
                noticeAddViewModel = this.noticeRepository.AddNotice(notice);
                return noticeAddViewModel;
            }
            else
            {
                noticeAddViewModel._failure = true;
                noticeAddViewModel._message = TOKENINVALID;
                return noticeAddViewModel;
            }

        }

        public NoticeAddViewModel UpdateNotice(NoticeAddViewModel notice)
        {
            NoticeAddViewModel noticeAddViewModel = new NoticeAddViewModel();
            if (TokenManager.CheckToken(notice._tenantName + notice._userName, notice._token))
            {
                noticeAddViewModel = this.noticeRepository.UpdateNotice(notice);
                return noticeAddViewModel;
            }
            else
            {
                noticeAddViewModel._failure = true;
                noticeAddViewModel._message = TOKENINVALID;
                return noticeAddViewModel;
            }

        }
        public NoticeDeleteModel DeleteNotice(NoticeDeleteModel notice)
        {
            NoticeDeleteModel noticdeleteModel = new NoticeDeleteModel();
            if (TokenManager.CheckToken(notice._tenantName + notice._userName, notice._token))
            {
                noticdeleteModel = this.noticeRepository.DeleteNotice(notice);
                return noticdeleteModel;
            }
            else
            {
                noticdeleteModel._failure = true;
                noticdeleteModel._message = TOKENINVALID;
                return noticdeleteModel;
            }

        }
        public NoticeAddViewModel ViewNotice(NoticeAddViewModel notice)
        {
            NoticeAddViewModel noticeAddViewModel = new NoticeAddViewModel();
            if (TokenManager.CheckToken(notice._tenantName + notice._userName, notice._token))
            {
                noticeAddViewModel = this.noticeRepository.ViewNotice(notice);
                //return getAllSchools();
                return noticeAddViewModel;

            }
            else
            {
                noticeAddViewModel._failure = true;
                noticeAddViewModel._message = TOKENINVALID;
                return noticeAddViewModel;
            }

        }
        public NoticeListViewModel GetAllNotice(NoticeListViewModel noticeList)
        {
            NoticeListViewModel getAllNoticeList = new NoticeListViewModel();
            if (TokenManager.CheckToken(noticeList._tenantName + noticeList._userName, noticeList._token))
            {
                getAllNoticeList = this.noticeRepository.GetAllNotice(noticeList);
                return getAllNoticeList;
            }
            else
            {
                getAllNoticeList = null;
                getAllNoticeList._failure = true;
                getAllNoticeList._message = TOKENINVALID;
                return getAllNoticeList;
            }
        }
       
    }
}
