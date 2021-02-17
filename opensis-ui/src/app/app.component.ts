import { Component, Inject, LOCALE_ID, Renderer2 } from '@angular/core';
import { ConfigService } from '../@vex/services/config.service';
import { Settings } from 'luxon';
import { DOCUMENT } from '@angular/common';
import { Platform } from '@angular/cdk/platform';
import { NavigationService } from '../@vex/services/navigation.service';
import icLayers from '@iconify/icons-ic/twotone-layers';
import icschool from '@iconify/icons-ic/baseline-account-balance';
import icinfo from '@iconify/icons-ic/info';
import icsettings from '@iconify/icons-ic/settings';
import icstudents from '@iconify/icons-ic/school';
import icusers from '@iconify/icons-ic/people';
import icschedule from '@iconify/icons-ic/date-range';
import icgrade from '@iconify/icons-ic/baseline-leaderboard';
import icattendance from '@iconify/icons-ic/baseline-access-alarm';
import icmessage from '@iconify/icons-ic/baseline-mark-email-unread';
import icactivity from '@iconify/icons-ic/accessibility';
import icreports from '@iconify/icons-ic/baseline-list-alt';
import ictools from '@iconify/icons-ic/baseline-construction';
import icparents from '@iconify/icons-ic/baseline-escalator-warning';
import icbook from '@iconify/icons-ic/baseline-book';
import { LayoutService } from '../@vex/services/layout.service';
import { ActivatedRoute } from '@angular/router';
import { filter, map } from 'rxjs/operators';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { SplashScreenService } from '../@vex/services/splash-screen.service';
import { Style, StyleService } from '../@vex/services/style.service';
import { ConfigName } from '../@vex/interfaces/config-name.model';

@Component({
  selector: 'vex-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'vex';

  constructor(private configService: ConfigService,
    private styleService: StyleService,
    private renderer: Renderer2,
    private platform: Platform,
    @Inject(DOCUMENT) private document: Document,
    @Inject(LOCALE_ID) private localeId: string,
    private layoutService: LayoutService,
    private route: ActivatedRoute,
    private navigationService: NavigationService,
    private splashScreenService: SplashScreenService) {
    Settings.defaultLocale = this.localeId;

    if (this.platform.BLINK) {
      this.renderer.addClass(this.document.body, 'is-blink');
    }

    /**
     * Customize the template to your needs with the ConfigService
     * Example:
     *  this.configService.updateConfig({
     *    sidenav: {
     *      title: 'Custom App',
     *      imageUrl: '//placehold.it/100x100',
     *      showCollapsePin: false
     *    },
     *    showConfigButton: false,
     *    footer: {
     *      visible: false
     *    }
     *  });
     */

    /**
     * Config Related Subscriptions
     * You can remove this if you don't need the functionality of being able to enable specific configs with queryParams
     * Example: example.com/?layout=apollo&style=default
     */
    this.route.queryParamMap.pipe(
      map(queryParamMap => queryParamMap.has('rtl') && coerceBooleanProperty(queryParamMap.get('rtl'))),
    ).subscribe(isRtl => {
      this.document.body.dir = isRtl ? 'rtl' : 'ltr';
      this.configService.updateConfig({
        rtl: isRtl
      });
    });

    this.route.queryParamMap.pipe(
      filter(queryParamMap => queryParamMap.has('layout'))
    ).subscribe(queryParamMap => this.configService.setConfig(queryParamMap.get('layout') as ConfigName));

    this.route.queryParamMap.pipe(
      filter(queryParamMap => queryParamMap.has('style'))
    ).subscribe(queryParamMap => this.styleService.setStyle(queryParamMap.get('style') as Style));


    this.navigationService.items = [
      {
        type: 'link',
        label: 'Dashboard',
        route: '/school/dashboards',
        icon: icLayers
      },
      {
        type: 'dropdown',
        label: 'Schools',
        icon: icschool,
        children: [
          {
            type: 'link',
            label: 'School Information',
            route: '/school/schoolinfo',
            icon: icinfo
          },
          {
            type: 'link',
            label: 'Marking Periods',
            route: '/school/marking-periods',
            icon: icinfo
          },
          {
            type: 'link',
            label: 'Calendars',
            route: '/school/schoolcalendars',
            icon: icinfo
          },
          {
            type: 'link',
            label: 'Notices',
            route: '/school/notices',
            icon: icinfo
          },
        ]
      },
      { type: 'dropdown',
        label: 'Students',
        icon: icstudents,
        children: [
          {
            type: 'link',
            label: 'Student Information',
            route: '/school/students',
            icon: icinfo
          },
          {
            type: 'link',
            label: 'Group Assign Student Info',
            route: '/school/students',
            icon: icinfo
          },
          {
            type: 'link',
            label: 'Student Re Enroll',
            route: '/school/students',
            icon: icinfo
          }
        ]
      },
      { type: 'link',
        label: 'Parents',
        icon: icparents,
        route: '/school/parents'       
      },
      { type: 'dropdown',
        label: 'Staff',
        icon: icusers,
        children: [
          {
            type: 'link',
            label: 'Staff Info',
            route: '/school/staff',
            icon: icinfo
          },
          {
            type: 'link',
            label: 'Teacher Functions',
            route: '/school/teacherfunctions',
            icon: icinfo
          }
        ]
      },
      { type: 'dropdown',
        label: 'Courses',
        icon: icbook,
        children: [
          {
            type: 'link',
            label: 'Course Manager',
            route: '/school/course-manager',
            icon: icinfo
          },
          {
            type: 'link',
            label: 'Course Catalog',
            route: '/school/coursemanager',
            icon: icinfo
          },
          {
            type: 'link',
            label: 'Teacher Reassignment',
            route: '/school/coursemanager',
            icon: icinfo
          }
        ]
      },
      { type: 'dropdown',
        label: 'Scheduling',
        icon: icschedule,
        children: [
          {
            type: 'link',
            label: 'Schedule Students',
            route: '/school/schedule-student',
            icon: icinfo
          },
          {
            type: 'link',
            label: 'Schedule Teacher',
            route: '/school/schedule-teacher',
            icon: icinfo
          },
          {
            type: 'link',
            label: 'Run Scheduler',
            route: '/school/runscheduler',
            icon: icinfo
          },
          {
            type: 'link',
            label: 'Group Drop',
            route: '/school/group-drop',
            icon: icinfo
          },
        ]
      },
      { type: 'dropdown',
        label: 'Grades',
        icon: icgrade,
        children: [
          {
            type: 'link',
            label: 'Progress Reports',
            route: '/school/progressreport',
            icon: icinfo
          }
        ]
      },
      { type: 'dropdown',
        label: 'Attendance',
        icon: icattendance,
        children: [
          {
            type: 'link',
            label: 'Administration',
            route: '/school/administration',
            icon: icinfo
          }
        ]
      },
      { type: 'dropdown',
        label: 'Messaging',
        icon: icmessage,
        children: [
          {
            type: 'link',
            label: 'Inbox',
            route: '/school/inbox',
            icon: icinfo
          }
        ]
      },
      { type: 'link',
        label: 'Reports',
        icon: icreports,
        route: '/school/progressreport'       
      },
      { type: 'link',
        label: 'Settings',
        icon: icsettings,
        route: '/school/settings'       
      },
      { type: 'dropdown',
        label: 'Tools',
        icon: ictools,
        children: [
          {
            type: 'link',
            label: 'Access Log',
            route: '/school/acesslog',
            icon: icinfo
          }
        ]
      },

    ];
  }
}
