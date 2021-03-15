import { Injectable } from '@angular/core';
import { NavigationDropdown, NavigationItem, NavigationLink, NavigationSubheading } from '../interfaces/navigation-item.interface';
import { BehaviorSubject, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NavigationService {

  items: NavigationItem[] = [];

  private _openChangeSubject = new Subject<NavigationDropdown>();
  openChange$ = this._openChangeSubject.asObservable();

  private changeMenuStatusTo = new BehaviorSubject<boolean>(false);
  menuItems = this.changeMenuStatusTo.asObservable();

  changeMenuItemsStatus(message:boolean) {
    this.changeMenuStatusTo.next(message)
  }

  // private menuItems = new BehaviorSubject(null);
  // menuStorage = this.menuItems.asObservable();

  // menuListStorage(list) {
  //   this.menuItems.next(list);
  // }
  constructor() { }

  triggerOpenChange(item: NavigationDropdown) {
    this._openChangeSubject.next(item);
  }

  isLink(item: NavigationItem): item is NavigationLink {
    return item.type === 'link';
  }

  isDropdown(item: NavigationItem): item is NavigationDropdown {
    return item.type === 'dropdown';
  }

  isSubheading(item: NavigationItem): item is NavigationSubheading {
    return item.type === 'subheading';
  }
}
