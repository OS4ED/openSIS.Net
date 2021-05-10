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
