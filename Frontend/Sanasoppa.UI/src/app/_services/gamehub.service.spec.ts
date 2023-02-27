import { TestBed } from '@angular/core/testing';

import { GameHubService } from './gamehub.service';

describe('GamehubService', () => {
  let service: GameHubService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GameHubService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
