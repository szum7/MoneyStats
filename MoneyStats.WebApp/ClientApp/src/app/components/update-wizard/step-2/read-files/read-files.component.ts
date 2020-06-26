import { Component, OnInit, Input } from '@angular/core';
import { ReadInBankRow } from 'src/app/models/component-models/read-in-bank-row';

@Component({
  selector: 'app-read-files-component',
  templateUrl: './read-files.component.html',
  styleUrls: ['./read-files.component.scss']
})
export class ReadFilesComponent implements OnInit {

  @Input() params: Array<Array<ReadInBankRow>>;

  constructor() {    
  }

  ngOnInit() {
  }

}
