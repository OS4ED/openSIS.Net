
import { Injectable } from '@angular/core';
import * as FileSaver from 'file-saver';
import * as XLSX from 'xlsx';
import * as _ from 'lodash';
import {formatDate} from '@angular/common';

const EXCEL_TYPE = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
const EXCEL_EXTENSION = '.xlsx';
@Injectable({
  providedIn: 'root'
})
export class ExcelService {

  constructor() { }
  public  exportAsExcelFile(json: any[], excelFileName: string): void {
    
    
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(json);
    this.autofitColumns(json, worksheet);
    const workbook: XLSX.WorkBook = { Sheets: { data: worksheet }, SheetNames: ['data'] };
    const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    this.saveAsExcelFile(excelBuffer, excelFileName);
  }
  private saveAsExcelFile(buffer: any, fileName: string): void {
    const data: Blob = new Blob([buffer], {
      type: EXCEL_TYPE
    });
    FileSaver.saveAs(data, fileName + formatDate(Date(), 'MM-dd-yyyy', 'en-US') + EXCEL_EXTENSION);
  }

  // Function to auto adjust space between elements 
  private autofitColumns(json: any[], worksheet: XLSX.WorkSheet, header?: string[]) {
    const jsonKeys = header ? header : Object.keys(json[0]);

    const objectMaxLength = [];
    for (let i = 0; i < json.length; i++) {
      const value = json[i];
      for (let j = 0; j < jsonKeys.length; j++) {
        if (typeof value[jsonKeys[j]] === 'number') {
          objectMaxLength[j] = 12;
        } else {
          const l = value[jsonKeys[j]] ? value[jsonKeys[j]].length : 0;
          objectMaxLength[j] =
            objectMaxLength[j] >= l
              ? objectMaxLength[j]
              : l;
        }
      }
      const key = jsonKeys;
      for (let j = 0; j < key.length; j++) {
        objectMaxLength[j] =
          objectMaxLength[j] >= key[j].length
            ? objectMaxLength[j]
            : key[j].length;
      }
    }
    const wscols = objectMaxLength.map(w => { return { width: w} });
    worksheet["!cols"] = wscols;
  }

  public sheetToJson(fileData){
    let workBook = null;
    let jsonData = null;

    workBook = XLSX.read(fileData, { type: 'binary',cellText:false,cellDates:true });
    jsonData = workBook.SheetNames.reduce((initial, name) => {
      const sheet = workBook.Sheets[name];
      initial[name] = XLSX.utils.sheet_to_json(sheet, {header:1,blankrows:false,raw:false,dateNF:'yyyy-mm-dd'});
      return initial;
    }, {});
    let firstKey = Object.keys(jsonData)[0]
    return jsonData[firstKey];
  }
}