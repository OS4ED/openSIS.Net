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

import { Component, OnInit, ViewChild } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { TranslateService } from "@ngx-translate/core";
import icSearch from "@iconify/icons-ic/search";
import { MatDatepicker, MatDatepickerInputEvent } from "@angular/material/datepicker";
 
@Component({
  selector: "vex-add-absences",
  templateUrl: "./add-absences.component.html",
  styleUrls: ["./add-absences.component.scss"],
})
export class AddAbsencesComponent implements OnInit {
  icSearch = icSearch;

  constructor(private dialog: MatDialog, public translateService: TranslateService) {
    translateService.use("en");
  }

  public CLOSE_ON_SELECTED = false;
  public init = new Date();
  public resetModel = new Date(0);
  public model = [
    new Date("4/5/2021"),
    new Date("4/12/2021"),
    new Date("4/18/2021"),
    new Date("4/19/2021"),
  ];
  @ViewChild("picker", { static: true }) _picker: MatDatepicker<Date>;

  public dateClass = (date: Date) => {
    if (this._findDate(date) !== -1) {
      return ["selected"];
    }
    return [];
  };

  public dateChanged(event: MatDatepickerInputEvent<Date>): void {
    if (event.value) {
      const date = event.value;
      const index = this._findDate(date);
      if (index === -1) {
        this.model.push(date);
      } else {
        this.model.splice(index, 1);
      }
      this.resetModel = new Date(0);
      if (!this.CLOSE_ON_SELECTED) {
        const closeFn = this._picker.close;
        this._picker.close = () => {};
        this._picker[
          "_popupComponentRef"
        ].instance._calendar.monthView._createWeekCells();
        setTimeout(() => {
          this._picker.close = closeFn;
        });
      }
    }
  }

  public remove(date: Date): void {
    const index = this._findDate(date);
    this.model.splice(index, 1);
  }

  private _findDate(date: Date): number {
    return this.model.map((m) => +m).indexOf(+date);
  }

  

  ngOnInit(): void {}
}
