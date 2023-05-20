import { lastValueFrom } from 'rxjs';
import { BusyService } from './busy.service';
import { Injectable } from '@angular/core';
import { environment as env } from '../../../environments/environment';
import {
  HttpTransportType,
  HubConnection,
  HubConnectionBuilder,
} from '@microsoft/signalr';
import { AuthService } from '@auth0/auth0-angular';

@Injectable({
  providedIn: 'root',
})
export class GameHubService {
  private _hubConnection?: HubConnection;
  private hubUrl = env.api.hubUrl;

  constructor(private busyService: BusyService, private auth: AuthService) {}

  public async startConnection(gameName: string) {
    this.busyService.busy();
    try {
      const token = await lastValueFrom(this.auth.getAccessTokenSilently());
      this._hubConnection = new HubConnectionBuilder()
        .withUrl(`${this.hubUrl}/gamehub=game=${gameName}`, {
          accessTokenFactory: () => token,
          transport: HttpTransportType.WebSockets,
        })
        .withAutomaticReconnect()
        .build();
      await this._hubConnection.start();
    } catch (err) {
      console.error(err);
    } finally {
      this.busyService.idle();
    }
  }

  get hubConnection(): HubConnection {
    if (!this._hubConnection) {
      throw Error('Gamehub is not initialized');
    }
    return this._hubConnection;
  }
}
