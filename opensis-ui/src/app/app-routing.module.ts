import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomLayoutComponent } from './custom-layout/custom-layout.component';
import { SettingsComponent } from './pages/settings/settings.component';
import { SchoolSettingsComponent } from './pages/settings/school-settings/school-settings.component';
import { StudentSettingsComponent } from './pages/settings/student-settings/student-settings.component';
import { StudentComponent } from './pages/student/studentinfo/student.component';
import { AddStudentComponent } from './pages/student/add-student/add-student.component';
import { 
  AuthGuard as AuthGuard
} from '../app/common/auth.guard';
import { VariableSchedulingComponent } from './pages/courses/course-manager/edit-course-section/variable-scheduling/variable-scheduling.component';
import { FixedSchedulingComponent } from './pages/courses/course-manager/edit-course-section/fixed-scheduling/fixed-scheduling.component';
import { RotatingSchedulingComponent } from './pages/courses/course-manager/edit-course-section/rotating-scheduling/rotating-scheduling.component';
import { CalendarDaysComponent } from './pages/courses/course-manager/edit-course-section/calendar-days/calendar-days.component';
const routes: Routes = [
  // { path: '', redirectTo: '/login', pathMatch: 'full' },
  {
    path: '',
    loadChildren: () => import('./pages/auth/login/login.module').then(m => m.LoginModule),
  },
  {
    path: ':id',
    loadChildren: () => import('./pages/auth/login/login.module').then(m => m.LoginModule),
  },  
  {
    path: 'school',
    component: CustomLayoutComponent,
    children: [
      {
        path: 'dashboards',
        loadChildren: () => import('./pages/dashboards/dashboard-analytics/dashboard-analytics.module').then(m => m.DashboardAnalyticsModule),
        canActivate: [AuthGuard]
      },
      {
        path: '',
        children: [
          {
            path: 'schoolinfo',
            loadChildren: () => import('./pages/school/school-details/school-details/school-details.module').then(m => m.SchoolDetailsModule),
            canActivate: [AuthGuard]
          }
        ]
      },
      {
        path: '',
        children: [
          {
            path: 'marking-periods',
            loadChildren: () => import('./pages/school/marking-periods/marking-periods.module').then(m => m.MarkingPeriodsModule),
            canActivate: [AuthGuard]
          }
        ]
      },
      {
        path: '',
        children: [
          {
            path: 'schoolcalendars',
            loadChildren: () => import('./pages/school/calendar/calendar.module').then(m => m.CalendarModule),
            canActivate: [AuthGuard]
          }
        ]
      },
      {
        path: '',
        children: [
          {
            path: 'notices',
            loadChildren: () => import('./pages/school/notices/notices.module').then(m => m.NoticesModule),
            canActivate: [AuthGuard]
          }
        ]
      },
      {
        path: '',
        children: [
          {
            path: 'students',
            loadChildren: () => import('./pages/student/studentinfo/student.module').then(m => m.StudentModule),
            canActivate: [AuthGuard]
            
          }
        ]
      },
      {
        path: '',
        children: [
          {
            path: 'studentdataimport',
            loadChildren: () => import('./pages/student/student-data-import/student-data-import.module').then(m => m.StudentDataImportModule),
            canActivate: [AuthGuard]
            
          }
        ]
      },
      {
        path: '',
        children: [
          {
            path: 'parents',
            loadChildren: () => import('./pages/parent/parentinfo/parentinfo.module').then(m => m.ParentinfoModule),
            //canActivate: [AuthGuard]            
          }
        ]
      },
      {
        path: '',
        children: [
          {
            path: 'staff',
            loadChildren: () => import('./pages/staff/staffinfo/staffinfo.module').then(m => m.StaffinfoModule),
            canActivate: [AuthGuard]            
          }
        ]
      },
      {
        path: '',
        children: [
          {
            path: 'course-manager',
            loadChildren: () => import('./pages/courses/course-manager/course-manager.module').then(m => m.CourseManagerModule),
            
          }
        ]
      },
      {
        path: '',
        children: [
          {
            path: 'schedule-teacher',
            loadChildren: () => import('./pages/scheduling/schedule-teacher/schedule-teacher.module').then(m => m.ScheduleTeacherModule),
            //canActivate: [AuthGuard]            
          }
        ]
      },
      {
        path: '',
        children: [
          {
            path: 'settings',
            loadChildren: () => import('./pages/settings/settings.module').then(m => m.SettingsModule),
            canActivate: [AuthGuard]            
            
          }
        ]
      },
      {
        path: '',
        children: [
          {
            path: 'schedule-student',
            loadChildren: () => import('./pages/scheduling/schedule-student/schedule-student.module').then(m => m.ScheduleStudentModule),
            // canActivate: [AuthGuard]            
          }
        ]
      },
      {
        path: '',
        children: [
          {
            path: 'group-drop',
            loadChildren: () => import('./pages/scheduling/group-drop/group-drop.module').then(m => m.GroupDropModule),
            // canActivate: [AuthGuard]            
          }
        ]
      },
      {
        path: '',
        children: [
          {
            path: 'teacher-reassignment',
            loadChildren: () => import('./pages/scheduling/teacher-reassignment/teacher-reassignment.module').then(m => m.TeacherReassignmentModule),
            // canActivate: [AuthGuard]            
          }
        ]
      },         
    ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    // preloadingStrategy: PreloadAllModules,
    scrollPositionRestoration: 'enabled',
    relativeLinkResolution: 'corrected',
    anchorScrolling: 'enabled'
  })],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
