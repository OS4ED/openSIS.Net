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
import { SchoolCreate } from '../../../../enums/school-create.enum';
import icCheckBoxOutlineBlank from '@iconify/icons-ic/check-box-outline-blank';
import icCheckBox from '@iconify/icons-ic/check-box';
import { StudentAddModel } from '../../../../models/student.model';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'vex-view-student-addressandcontacts',
  templateUrl: './view-student-addressandcontacts.component.html',
  styleUrls: ['./view-student-addressandcontacts.component.scss'],
  animations: [
    stagger60ms
  ],
})
export class ViewStudentAddressandcontactsComponent implements OnInit {
  icCheckBoxOutlineBlank = icCheckBoxOutlineBlank;
  icCheckBox = icCheckBox;

  @Input() studentCreateMode: SchoolCreate;
  @Input() studentViewDetails: StudentAddModel;
  @Input() nameOfMiscValues;
  constructor(private snackbar: MatSnackBar) { }

  showHomeAddressOnGoogleMap() {
    const stAdd1 = this.studentViewDetails.studentMaster.homeAddressLineOne;
    const stAdd2 = this.studentViewDetails.studentMaster.homeAddressLineTwo;
    const city = this.studentViewDetails.studentMaster.homeAddressCity;
    const country = this.nameOfMiscValues.countryName;
    const state = this.studentViewDetails.studentMaster.homeAddressState;
    const zip = this.studentViewDetails.studentMaster.homeAddressZip;
    const homeAddressMapUrl = `https://maps.google.com/?q=${stAdd1 ? stAdd1 : ''}${stAdd2 ? ',' + stAdd2 : ''}${city ? ',' + city : ''}${state ? ',' + state : ''}${zip ? ',' + zip : ''}${country ? ',' + country : ''}`;
    window.open(homeAddressMapUrl, '_blank');

  }
  showMailingAddressOnGoogleMap() {
    const stAdd1 = this.studentViewDetails.studentMaster.mailingAddressLineOne;
    const stAdd2 = this.studentViewDetails.studentMaster.mailingAddressLineTwo;
    const city = this.studentViewDetails.studentMaster.mailingAddressCity;
    const country = this.nameOfMiscValues.mailingAddressCountry;
    const state = this.studentViewDetails.studentMaster.mailingAddressState;
    const zip = this.studentViewDetails.studentMaster.mailingAddressZip;

    const mailingAddressMapUrl = `https://maps.google.com/?q=${stAdd1 ? stAdd1 : ''}${stAdd2 ? ',' + stAdd2 : ''}${city ? ',' + city : ''}${state ? ',' + state : ''}${zip ? ',' + zip : ''}${country ? ',' + country : ''}`;
    window.open(mailingAddressMapUrl, '_blank');

  }

  ngOnInit(): void {
    this.nameOfMiscValues.countryName = this.nameOfMiscValues.countryName === '-' ? null : this.nameOfMiscValues.countryName;
    this.nameOfMiscValues.mailingAddressCountry = this.nameOfMiscValues.mailingAddressCountry === '-' ? null : this.nameOfMiscValues.mailingAddressCountry;
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
