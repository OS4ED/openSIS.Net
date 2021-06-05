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

import { Component, OnInit } from '@angular/core';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDeleteForever from '@iconify/icons-ic/twotone-delete-forever';
import icDelete from '@iconify/icons-ic/twotone-delete';
import { MatDialog } from '@angular/material/dialog';
import { AddAssignmentComponent } from './add-assignment/add-assignment.component';
import { CreateAssignmentComponent } from './create-assignment/create-assignment.component';
import { DeleteAssignmentsComponent } from './delete-assignments/delete-assignments.component';

@Component({
  selector: 'vex-assignments',
  templateUrl: './assignments.component.html',
  styleUrls: ['./assignments.component.scss']
})
export class AssignmentsComponent implements OnInit {

  icEdit = icEdit;
  icDeleteForever = icDeleteForever;
  icDelete = icDelete;

  constructor(private dialog: MatDialog) {}

  ngOnInit(): void {
  }

  addAssignment(){
    this.dialog.open(AddAssignmentComponent, {
      width: '500px'
    });
  }

  createAssignment(){
    this.dialog.open(CreateAssignmentComponent, {
      width: '800px'
    });
  }

  deleteAssignment(){
    this.dialog.open(DeleteAssignmentsComponent, {
      width: '500px'
    });
  }

}
