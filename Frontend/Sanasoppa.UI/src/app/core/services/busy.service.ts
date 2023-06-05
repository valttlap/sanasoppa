import { Injectable } from '@angular/core';
import { NgxSpinnerService, Spinner } from 'ngx-spinner';

@Injectable({
  providedIn: 'root',
})
export class BusyService {
  private busyRequestCount = 0;
  private readonly spinner: Spinner = {
    type: 'line-scale-party',
    bdColor: 'rgba(255,255,255,0)',
    color: '#bdc3c7',
  };

  constructor(private spinnerService: NgxSpinnerService) {}

  busy(): void {
    this.busyRequestCount++;
    this.spinnerService.show(undefined, this.spinner);
  }

  idle(): void {
    this.busyRequestCount--;
    if (this.busyRequestCount <= 0) {
      this.busyRequestCount = 0;
      this.spinnerService.hide;
    }
  }
}
