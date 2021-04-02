import { Component, Input, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SchoolCreate } from '../../../../enums/school-create.enum';
import { SchoolAddViewModel } from '../../../../models/schoolMasterModel';

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
