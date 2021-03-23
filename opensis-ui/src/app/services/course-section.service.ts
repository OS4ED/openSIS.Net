import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { environment } from '../../environments/environment';
import { SearchCourseSectionViewModel } from '../models/courseManagerModel';
import { CourseSectionAddViewModel, DeleteCourseSectionSchedule, GetAllCourseSectionModel, GetAllCourseStandardForCourseSectionModel} from '../models/courseSectionModel';
import { CryptoService } from './Crypto.service';

@Injectable({
  providedIn: 'root'
})
export class CourseSectionService {
  apiUrl: string = environment.apiURL;
  constructor(private http: HttpClient, private cryptoService: CryptoService) { }

     getAllCourseSection(courseSection: GetAllCourseSectionModel) {
      
        let apiurl = this.apiUrl + courseSection._tenantName + "/CourseManager/getAllCourseSection";
        return this.http.post<GetAllCourseSectionModel>(apiurl, courseSection)
    }
    addCourseSection(courseSection : CourseSectionAddViewModel){
      let apiurl = this.apiUrl + courseSection._tenantName + "/CourseManager/addCourseSection";
      return this.http.post<GetAllCourseSectionModel>(apiurl, courseSection)
    }
    updateCourseSection(courseSection : CourseSectionAddViewModel){
      let apiurl = this.apiUrl + courseSection._tenantName + "/CourseManager/updateCourseSection";
      return this.http.put<GetAllCourseSectionModel>(apiurl, courseSection)
    }
    deleteCourseSection(courseSection : CourseSectionAddViewModel){
      let apiurl = this.apiUrl + courseSection._tenantName + "/CourseManager/deleteCourseSection";
      return this.http.post<CourseSectionAddViewModel>(apiurl, courseSection)
    }

    private dataSource = new Subject;
    currentUpdate = this.dataSource.asObservable();

    sendCurrentData(message: boolean) {
      this.dataSource.next(message)
    }

    getAllCourseStandardForCourseSection(courseSection : GetAllCourseStandardForCourseSectionModel){
      let apiurl = this.apiUrl + courseSection._tenantName + "/CourseManager/getAllCourseStandardForCourseSection";
      return this.http.post<GetAllCourseStandardForCourseSectionModel>(apiurl, courseSection)
    }

    deleteSchedule(courseSection : DeleteCourseSectionSchedule){
      let apiurl = this.apiUrl + courseSection._tenantName + "/CourseManager/deleteSchedule";
      return this.http.post<DeleteCourseSectionSchedule>(apiurl, courseSection)
    }

    

    private afterDeleted = new Subject;
    callCourseSection = this.afterDeleted.asObservable();

    scheduleDeleted(message: boolean) {
      this.afterDeleted.next(message)
    }

    searchCourseSectionForSchedule(courseSection : SearchCourseSectionViewModel){
      let apiurl = this.apiUrl + courseSection._tenantName + "/CourseManager/searchCourseSectionForSchedule";
      return this.http.post<SearchCourseSectionViewModel>(apiurl, courseSection)
    }
}
