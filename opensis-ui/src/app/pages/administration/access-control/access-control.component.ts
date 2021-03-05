import { Component, OnInit, ViewChild, Input, OnChanges, SimpleChanges } from '@angular/core';
import { LayoutService } from 'src/@vex/services/layout.service';
import icExpandAll from '@iconify/icons-ic/unfold-more';
import icCollapseAll from '@iconify/icons-ic/unfold-less';
import icExpand from '@iconify/icons-ic/expand-more';
import icCollapse from '@iconify/icons-ic/expand-less';
import { MatAccordion } from '@angular/material/expansion';
import { RollBasedAccessService } from '../../../services/rollBasedAccess.service';
import { RolePermissionListViewModel,RolePermissionViewModel,PermissionCategory ,PermissionSubCategory,PermissionGroupListViewModel} from '../../../models/rollBasedAccessModel';
import { NgForm } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'vex-access-control',
  templateUrl: './access-control.component.html',
  styleUrls: ['./access-control.component.scss']
})
export class AccessControlComponent implements OnInit,OnChanges {
  
  @ViewChild(MatAccordion) accordion: MatAccordion;
  @Input()  data;
  @ViewChild('form') currentForm: NgForm;
  icExpandAll = icExpandAll;
  icExpand = icExpand;
  icCollapseAll = icCollapseAll;
  icCollapse = icCollapse;
  permissionList =[];
  permissionGroupList=[];
  allCanViewCheckFlag=false;
  memberId;
  rolePermissionListViewModel:RolePermissionListViewModel= new RolePermissionListViewModel();
  permissionGroupListViewModel:PermissionGroupListViewModel= new PermissionGroupListViewModel();
  constructor(
    private layoutService: LayoutService,
    private rollBasedAccessService:RollBasedAccessService,
    private snackbar: MatSnackBar
    ) {
    this.layoutService.collapseSidenav();
    this.rollBasedAccessService.selectedMember.subscribe(member=>{
       this.memberId = member;
    })
    this.rollBasedAccessService.currentpermissionList.subscribe(receiveddata=>{
      this.permissionList = receiveddata; 
      this.permissionGroupListViewModel.permissionGroupList = [];
      this.permissionList.map((val,index)=>{ 
          this.permissionGroupListViewModel.permissionGroupList.push(val.permissionGroup)               
          this.rolePermissionListViewModel.permissionList.push(new RolePermissionViewModel())
       
        val.permissionGroup.permissionCategory.map((val1,j)=>{         
          this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory.push(new PermissionCategory());            
            val1.permissionSubcategory.map((val2,k)=>{             
              this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].permissionSubcategory.push(new PermissionSubCategory())
            })          
        })
      })
     
     this.permissionList.map((val,index)=>{   
        this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionGroupName= val.permissionGroup.permissionGroupName;   
        val.permissionGroup.permissionCategory.map((val1,j)=>{          
          this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].permissionCategoryName= val1.permissionCategoryName;
          this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].rolePermission[0].canView = val1.rolePermission[0].canView;
          this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].rolePermission[0].canEdit = val1.rolePermission[0].canEdit;
          this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].rolePermission[0].membershipId = this.memberId;
          if (Array.isArray(val1.permissionSubcategory) && val1.permissionSubcategory.length){
             val1.permissionSubcategory.map((val2,k)=>{  
              if(val2.rolePermission[0]!== undefined && this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].permissionSubcategory[k] !== undefined){
                this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].permissionSubcategory[k].permissionSubcategoryName = val2.permissionSubcategoryName;              
                this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].permissionSubcategory[k].rolePermission[0].canView = val2.rolePermission[0].canView;
                this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].permissionSubcategory[k].rolePermission[0].canEdit = val2.rolePermission[0].canEdit;
                this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].permissionSubcategory[k].rolePermission[0].membershipId = this.memberId;
              }
            })
          }       
        
       })      
       
     })  
     var getAllPermissionList = this.rolePermissionListViewModel.permissionList.filter(function(val) {
      return val.permissionGroup.hasOwnProperty('permissionGroupName')
    });
    var addPermissionList = this.permissionGroupListViewModel.permissionGroupList.filter(function(val) {
      return val.hasOwnProperty('permissionGroupName')
    });
    this.rolePermissionListViewModel.permissionList = getAllPermissionList;
    
    this.permissionGroupListViewModel.permissionGroupList=addPermissionList;
    
   
    /*this.rolePermissionListViewModel.permissionList.map((val,i)=>{
      var fl = true;
      val.permissionGroup.permissionCategory.map((val,j)=>{       
        if((val.rolePermission[0].canView === true || val.rolePermission[0].canEdit === true) && fl){         
          this.rolePermissionListViewModel.permissionList[i].permissionGroup.isActive = true;
          fl = false;
        }         
      })        
    })  */
     
    }) 
  } 

  ngOnInit(): void {
   
  }
  ngOnChanges(changes: SimpleChanges){
    this.permissionList
  }
  getRolePermission(memberId){  
    this.rolePermissionListViewModel.membershipId = memberId;
    this.rolePermissionListViewModel.permissionList.map((val,i)=>{
      val.permissionGroup.permissionCategory.map((val1,j)=>{
        delete this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].rolePermission[0].membershipId
        val1.permissionSubcategory.map((val2,k)=>{
          delete this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].permissionSubcategory[k].rolePermission[0].membershipId
        })
      })
    })
    this.rollBasedAccessService.getAllRolePermission(this.rolePermissionListViewModel).subscribe(
      (res) => {
      if (typeof (res) == 'undefined') {
        this.snackbar.open('Role Permission List failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (res._failure) {
          if (res._message === "NO RECORD FOUND") {
            if (res.permissionList == null) {
              this.permissionList=null;
            }

          } else {
            this.snackbar.open('Role Permission List failed. ' + res._message, 'LOL THANKS', {
              duration: 10000
            });
          }

        }
        else {
         this.permissionList = res.permissionList;        
         
        }
      }
    })
  }
  changeToFalseOfCanView(val,i,j){  
    if(val === false){
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].permissionSubcategory.map((res,k)=>{       
        this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].permissionSubcategory[k].rolePermission[0].canView = true;
                
      })
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].rolePermission[0].canView = true;
    }    
  }
  changeToFalseOfCanEdit(val,i,j){    
    
    this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].permissionSubcategory.map((res,k)=>{        
      if(val === true){
        this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].permissionSubcategory[k].rolePermission[0].canView = false;
        this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].permissionSubcategory[k].rolePermission[0].canEdit = false;
      }else if(val === false){
        this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].permissionSubcategory[k].rolePermission[0].canView = true;
      }
    })
    if(val === true){
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].rolePermission[0].canEdit = false;
    } 
  }
  changeCategoryCanView(e,val){ 
   
    let index=this.rolePermissionListViewModel.permissionList.findIndex((x)=>{     
      return x.permissionGroup.permissionGroupName===val.permissionGroup.permissionGroupName
    })     

    this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory.map((res,j)=>{        
      if(e.source.checked){
        this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].rolePermission[0].canView = true;
        this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].permissionSubcategory.map((res1,k)=>{
          this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].permissionSubcategory[k].rolePermission[0].canView = true;
        })
      }else{       
        this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].rolePermission[0].canView = false;
        this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].rolePermission[0].canEdit = false;
        this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].permissionSubcategory.map((res1,k)=>{
          this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].permissionSubcategory[k].rolePermission[0].canView = false;
          this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].permissionSubcategory[k].rolePermission[0].canEdit = false;
        })
        this.rolePermissionListViewModel.permissionList[index].permissionGroup.isActive = false;
      }  
    })
      
  }
  changeCategoryCanEdit(e,val){   
    let index=this.rolePermissionListViewModel.permissionList.findIndex((x)=>{     
      return x.permissionGroup.permissionGroupName===val.permissionGroup.permissionGroupName
    })
    this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory.map((res,j)=>{        
      if(e.source.checked){
        this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].rolePermission[0].canEdit = true;
        this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].rolePermission[0].canView = true;

        this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].permissionSubcategory.map((res1,k)=>{
          this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].permissionSubcategory[k].rolePermission[0].canView = true;
          this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].permissionSubcategory[k].rolePermission[0].canEdit = true;
        })      
      }else{
        this.permissionGroupListViewModel.permissionGroupList[index].permissionCategory[j].rolePermission[0].canEdit = false;
       
      }  
    })
  }
  changeSubCategoryCanView(i,j,k,e){    
    if(e.checked === false){      
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].permissionSubcategory[k].rolePermission[0].canEdit = false;
    }else{
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].rolePermission[0].canView = true;
    }    
  }

  changeSubCategoryCanEdit(i,j,k,e){   
    if(e.checked){
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].permissionSubcategory[k].rolePermission[0].canView = true;
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].rolePermission[0].canView = true;
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].rolePermission[0].canEdit = true;
    }    
  }
  
  submit(){   
   
    this.rolePermissionListViewModel.permissionList.map((val,i)=>{
      if(val.permissionGroup !== undefined){
        val.permissionGroup.permissionCategory.map((val,j)=>{       
          if(val.permissionCategoryName !== undefined && this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j] !== undefined){         
            this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].rolePermission[0].canEdit= val.rolePermission[0].canEdit;
            this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].rolePermission[0].canView= val.rolePermission[0].canView;
            val.permissionSubcategory.map((val1,k)=>{     
              if(val1.permissionSubcategoryName!== undefined && this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].permissionSubcategory[k] !== undefined){
                if(val1.rolePermission[0] !== undefined && this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].permissionSubcategory[k].rolePermission[0] !== undefined){
                  this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].permissionSubcategory[k].rolePermission[0].canEdit= val1.rolePermission[0].canEdit;
                  this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].permissionSubcategory[k].rolePermission[0].canView= val1.rolePermission[0].canView;
                }
                
              }
            })         
          }           
        })       
      }
       
    })   
  
   this.rollBasedAccessService.updateRolePermission(this.permissionGroupListViewModel).subscribe(
      (res) => {
        if(typeof(res)=='undefined'){
          this.snackbar.open('Role Base Access Submission failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else{
          if (res._failure) {
            this.snackbar.open('Role Base Access Submission failed. ' + res._message, 'LOL THANKS', {
              duration: 10000
            });
          } 
          else{       
            
            this.snackbar.open('Role Base Access Submission Successful.', '', {
              duration: 10000
            })
           this.getRolePermission(this.memberId);
           
          }        
        }      
      });
    
        
  }
    
  
   
}
