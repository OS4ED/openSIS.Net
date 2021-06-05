/***********************************************************************************
openSIS is a free student information system for public and non-public
schools from Open Solutions for Education, Inc.Website: www.os4ed.com.

Visit the openSIS product website at https://opensis.com to learn more.
If you have question regarding this software or the license, please contact
via the website.

The software is released under the terms of the GNU Affero General Public License as
published by the Free Software Foundation, version 3 of the License.
See https://www.gnu.org/licenses/agpl-3.0.en.html.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

Copyright (c) Open Solutions for Education, Inc.

All rights reserved.
***********************************************************************************/

import { Component, Input, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { StaffAddModel } from '../../../../models/staff.model';
import icCheckBox from '@iconify/icons-ic/check-box';
import icCheckBoxOutlineBlank from '@iconify/icons-ic/check-box-outline-blank';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { MatSnackBar } from '@angular/material/snack-bar';
import { StaffService } from 'src/app/services/staff.service';

@Component({
  selector: 'vex-view-staff-addressinfo',
  templateUrl: './view-staff-addressinfo.component.html',
  styleUrls: ['./view-staff-addressinfo.component.scss'],
  animations: [
    stagger60ms
  ]
})


export class ViewStaffAddressinfoComponent implements OnInit {
  @Input() staffCreateMode;
  @Input() categoryId;
  @Input() staffViewDetails: StaffAddModel;
  @Input() nameOfMiscValues;
  module = 'Staff';
  icCheckBox = icCheckBox;
  icCheckBoxOutlineBlank = icCheckBoxOutlineBlank;
  homeAddressMapUrl: string;
  mailingAddressMapUrl: string;
  constructor(
              public translateService: TranslateService,
              private snackbar: MatSnackBar,
              private staffService: StaffService) {
    translateService.use('en');
  }

  ngOnInit(): void {

 

    this.nameOfMiscValues.countryName=this.nameOfMiscValues.countryName=='-'?'':this.nameOfMiscValues.countryName;
    this.nameOfMiscValues.mailingAddressCountry=this.nameOfMiscValues.mailingAddressCountry=='-'?'':this.nameOfMiscValues.mailingAddressCountry;

  }
  showHomeAddressOnGoogleMap(){
    let stAdd1 = this.staffViewDetails.staffMaster.homeAddressLineOne;
    let stAdd2 = this.staffViewDetails.staffMaster.homeAddressLineTwo;
    let city = this.staffViewDetails.staffMaster.homeAddressCity;
    let country = this.nameOfMiscValues.countryName;
    let state = this.staffViewDetails.staffMaster.homeAddressState;
    let zip = this.staffViewDetails.staffMaster.homeAddressZip;

      this.homeAddressMapUrl = `https://maps.google.com/?q=${stAdd1?stAdd1:''}${stAdd2?','+stAdd2:''}${city?','+city:''}${state?','+state:''}${country?','+country:''}${zip?','+zip:''}`;
      window.open(this.homeAddressMapUrl, '_blank');
    
  }
  showMailingAddressOnGoogleMap(){
    let stAdd1 = this.staffViewDetails.staffMaster.mailingAddressLineOne;
    let stAdd2 = this.staffViewDetails.staffMaster.mailingAddressLineTwo;
    let city = this.staffViewDetails.staffMaster.mailingAddressCity;
    let country = this.nameOfMiscValues.mailingAddressCountry;
    let state = this.staffViewDetails.staffMaster.mailingAddressState;
    let zip = this.staffViewDetails.staffMaster.mailingAddressZip;
      this.mailingAddressMapUrl = `https://maps.google.com/?q=${stAdd1?stAdd1:''}${stAdd2?','+stAdd2:''}${city?','+city:''}${state?','+state:''}${country?','+country:''}${zip?','+zip:''}`;
      window.open(this.mailingAddressMapUrl, '_blank');
    
  }

  openUrl(href: string) {
    if (!href?.trim()) { return; }

    if (href.includes('@')) {
      href = 'mailto:' + href;
      window.open(href);
      return;
    } else
      if (!href.includes('http:') && !href.includes('https:')) {
        href = 'http://' + href;
      }
    window.open(href);
  }

}
