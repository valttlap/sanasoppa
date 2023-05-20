import { BusyService } from './busy.service';
import { Injectable } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import {
  HttpTransportType,
  HubConnection,
  HubConnectionBuilder,
} from '@microsoft/signalr';
import { lastValueFrom } from 'rxjs';
import { environment as env } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class LobbyHubService {
  private _hubConnection?: HubConnection;
  private _hubUrl = env.api.hubUrl;

  constructor(private busyService: BusyService, private auth: AuthService) {}

  public async startConnection(): Promise<void> {
    this.busyService.busy();
    try {
      const token = await lastValueFrom(this.auth.getAccessTokenSilently());
      this._hubConnection = new HubConnectionBuilder()
        .withUrl(`${this._hubUrl}/lobbyhub`, {
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
      throw Error('Lobbyhub connection is not initialized');
    }
    return this._hubConnection;
  }

  public disconnect(): Promise<void> {
    if (!this._hubConnection) {
      throw Error('Lobbyhub connection is not initialized');
    }
    return this._hubConnection.stop();
  }
}
