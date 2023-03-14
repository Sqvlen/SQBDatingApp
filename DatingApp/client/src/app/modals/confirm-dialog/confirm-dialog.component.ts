import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css']
})
export class ConfirmDialogComponent implements OnInit {
  title = '';
  message = '';
  btnOkText = '';
  btnCancelText = '';
  result: boolean = false;
  
  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }

  confirm() {
    this.result = true;
    this.bsModalRef.hide();
  }

  decline() {
    this.bsModalRef.hide();
  }
}
