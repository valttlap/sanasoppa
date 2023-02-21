import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class GameHubService {
  private hubConnection?: HubConnection;
  private apiUrl = environment.apiUrl;

  startConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.apiUrl + '/hubs/gamehub')
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connected to gamehub'))
      .catch(err => console.log('Error while starting connection: ' + err));
  }

  getHubConnection(): HubConnection {
    if (!this.hubConnection) {
      throw Error('Gamehub connection is not initialized');
    }
    return this.hubConnection;
  }
}
