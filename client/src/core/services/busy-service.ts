import { CurrencyPipe } from '@angular/common';
import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BusyService {
  busyRequestCont = signal(0);

  busy() {
    this.busyRequestCont.update(current => current + 1 );

  }

  idle() {
    this.busyRequestCont.update(current => Math.max(0, current-1));
  }
}
