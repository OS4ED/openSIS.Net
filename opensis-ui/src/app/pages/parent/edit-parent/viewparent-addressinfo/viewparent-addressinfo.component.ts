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
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { fadeInRight400ms } from '../../../../../@vex/animations/fade-in-right.animation';
import { TranslateService } from '@ngx-translate/core';
import icEdit from '@iconify/icons-ic/edit';
import icDelete from '@iconify/icons-ic/delete';
import icCheckBox from '@iconify/icons-ic/check-box';
import icCheckCircle from '@iconify/icons-ic/twotone-done';
import icCheckBoxOutlineBlank from '@iconify/icons-ic/check-box-outline-blank';

@Component({
  selector: 'vex-viewparent-addressinfo',
  templateUrl: './viewparent-addressinfo.component.html',
  styleUrls: ['./viewparent-addressinfo.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms,
    fadeInRight400ms
  ]
})
export class ViewparentAddressinfoComponent implements OnInit {
  icEdit = icEdit;
  icDelete = icDelete;
  icCheckBox = icCheckBox;
  icCheckBoxOutlineBlank = icCheckBoxOutlineBlank;
  icCheckCircle = icCheckCircle;

  constructor(public translateService:TranslateService) {
    translateService.use('en');
  }

  ngOnInit(): void {
  }

}
