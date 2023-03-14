import { Injectable } from '@angular/core';
import { LoaderService } from './loader.service';
//import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  public busyRequestCount = 0;

  constructor(private loadingService: LoaderService) { }


  busy(): void
  {
    this.busyRequestCount++;
    this.loadingService.setLoading(true);
  }

  idle()
  {
    this.busyRequestCount--;
    if (this.busyRequestCount <= 0)
    {
      this.busyRequestCount = 0;
      this.loadingService.setLoading(false);
    }
  }
}
