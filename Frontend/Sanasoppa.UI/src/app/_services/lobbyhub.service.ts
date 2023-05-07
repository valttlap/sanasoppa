import { BusyService } from './busy.service';
import { Injectable } from '@angular/core';
import { User } from '@auth0/auth0-angular';
import {
  HttpTransportType,
  HubConnection,
  HubConnectionBuilder,
} from '@microsoft/signalr';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class LobbyHubService {
  private hubConnection?: HubConnection;
  private hubUrl = environment.api.hubUrl;

  constructor(private busyService: BusyService) {}

  startConnection(user: User) {
    //TODO: FIX AUTHENTICATION
    /*     this.busyService.busy();
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${this.hubUrl}/lobbyhub`, {
        accessTokenFactory: () => user.token,
        transport: HttpTransportType.WebSockets,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .catch(err => console.error(err))
      .finally(() => this.busyService.idle()); */
  }

  getHubConnection(): HubConnection {
    if (!this.hubConnection) {
      throw Error('Lobbyhub connection is not initialized');
    }
    return this.hubConnection;
  }

  disconnect(): Promise<void> {
    if (!this.hubConnection) {
      throw Error('Lobbyhub connection is not initialized');
    }
    return this.hubConnection.stop();
  }
}
