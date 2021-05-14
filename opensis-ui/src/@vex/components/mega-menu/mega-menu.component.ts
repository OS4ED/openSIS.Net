import { Component, OnInit } from '@angular/core';
import { Icon } from '@visurel/iconify-angular';
import icSchool from '@iconify/icons-ic/twotone-account-balance';
import icStudents from '@iconify/icons-ic/twotone-school';
import icStaff from '@iconify/icons-ic/twotone-people';
import icEvent from '@iconify/icons-ic/twotone-event';
import icNotice from '@iconify/icons-ic/twotone-assignment';
import { PopoverRef } from '../popover/popover-ref';

export interface MegaMenuFeature {
  icon?: Icon;
  label: string;
  route: string;
}

export interface MegaMenuPage {
  label: string;
  route: string;
}

@Component({
  selector: 'vex-mega-menu',
  templateUrl: './mega-menu.component.html'
})
export class MegaMenuComponent implements OnInit {

  features: MegaMenuFeature[] = [
    {
      icon: icSchool,
      label: 'School',
      route: './school/schoolinfo/generalinfo'
    },
    {
      icon: icStudents,
      label: 'Student',
      route: './school/students/student-generalinfo'
    },
    {
      icon: icStaff,
      label: 'Staff',
      route: './school/staff/staff-generalinfo'
    },
    {
      icon: icEvent,
      label: 'Event',
      route: './school/schoolcalendars'
    },
    {
      icon: icNotice,
      label: 'Notice',
      route: './school/notices'
    }
  ];

  pages: MegaMenuPage[] = [
    {
      label: 'All-In-One Table',
      route: '/apps/aio-table'
    },
    {
      label: 'Authentication',
      route: '/login'
    },
    {
      label: 'Components',
      route: '/ui/components/overview'
    },
    {
      label: 'Documentation',
      route: '/documentation'
    },
    {
      label: 'FAQ',
      route: '/pages/faq'
    },
    {
      label: 'Form Elements',
      route: '/ui/forms/form-elements'
    },
    {
      label: 'Form Wizard',
      route: '/ui/forms/form-wizard'
    },
    {
      label: 'Guides',
      route: '/pages/guides'
    },
    {
      label: 'Help Center',
      route: '/apps/help-center'
    },
    {
      label: 'Scrumboard',
      route: '/apps/scrumboard'
    }
  ];

  constructor(private popoverRef: PopoverRef<MegaMenuComponent>) {
    // popoverRef.data[0]?.permissionGroup?.permissionCategory?.map((item) => {
    //   if(item.rolePermission[0].canView){
    //     this.features.push({
    //       icon: this.pickIcon(item.permissionCategoryName),
    //       label: item.title,
    //       route: item.path
    //     });
    //   }
    // });
  }

  // pickIcon(categoryName) {
  //   switch (categoryName) {
  //     case 'School':{
  //       return icSchool;
  //     }
  //     case 'Student':{
  //       return icStudents;

  //     }
  //     case 'Staff':{
  //       return icStaff;

  //     }
  //     case 'Event':{
  //       return icEvent;

  //     }
  //     case 'Notice':{
  //       return icNotice;
  //     }
  //   }
  // }
  ngOnInit() {
  }

  close() {
    this.popoverRef.close();
  }
}
