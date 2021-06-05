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
import { MatSnackBar } from '@angular/material/snack-bar';
import { SchoolCreate } from '../../../../enums/school-create.enum';
import { SchoolAddViewModel } from '../../../../models/school-master.model';

@Component({
  selector: 'vex-view-general-info',
  templateUrl: './view-general-info.component.html',
  styleUrls: ['./view-general-info.component.scss']
})

export class ViewGeneralInfoComponent implements OnInit {
  @Input() schoolCreateMode: SchoolCreate;
  @Input() categoryId;
  @Input() schoolViewDetails: SchoolAddViewModel;
  module = 'School';
  status: string;
  mapUrl:string;
  mailText:string;
  constructor(private snackbar: MatSnackBar) {
  }

  ngOnInit(): void {
    if (this.schoolViewDetails.schoolMaster.schoolDetail[0].status != null) {
      this.status = this.schoolViewDetails.schoolMaster.schoolDetail[0].status ? 'Active' : 'Inactive';
    }
  }

  showOnGoogleMap(){
    let stAdd1 = this.schoolViewDetails.schoolMaster.streetAddress1;
    let stAdd2 = this.schoolViewDetails.schoolMaster.streetAddress2;
    let city = this.schoolViewDetails.schoolMaster.city;
    let country = this.schoolViewDetails.schoolMaster.country;
    let state = this.schoolViewDetails.schoolMaster.state;
    let zip = this.schoolViewDetails.schoolMaster.zip;
    let longitude=this.schoolViewDetails.schoolMaster.longitude;
    let latitude=this.schoolViewDetails.schoolMaster.latitude;
    this.mapUrl = `https://maps.google.com/?q=${stAdd1?stAdd1:''}${stAdd2?','+stAdd2:''}${city?','+city:''}${state?','+state:''}${zip?','+zip:''}${country?','+country:''}`;
    window.open(this.mapUrl, '_blank');
  }
  goToWebsite(){
    this.urlFormatter(this.schoolViewDetails?.schoolMaster.schoolDetail[0].website);
  }
  goToTwitter(){
    this.urlFormatter(this.schoolViewDetails?.schoolMaster.schoolDetail[0].twitter)
  }
  goToFacebook(){
    this.urlFormatter(this.schoolViewDetails?.schoolMaster.schoolDetail[0].facebook)
  }

  goToInstagram(){ 
    this.urlFormatter(this.schoolViewDetails?.schoolMaster.schoolDetail[0].instagram)
  }
  goToYoutube(){
    this.urlFormatter(this.schoolViewDetails?.schoolMaster.schoolDetail[0].youtube)
  }
  goToLinkedin(){
    this.urlFormatter(this.schoolViewDetails?.schoolMaster.schoolDetail[0].linkedIn);
  }
  goToEmail(){
    this.mailText = "mailto:"+this.schoolViewDetails?.schoolMaster.schoolDetail[0].email;
    window.open(this.mailText, "_blank");
    
  }

  urlFormatter(fullUrl){
    let arrUrl=fullUrl.split(':'); 
    if(arrUrl.length>1){
      window.open(fullUrl, "_blank");
    }
    else{
      window.open(`http://${fullUrl}`, "_blank");
    }
  }

}
