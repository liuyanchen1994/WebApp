import { TestBed, inject } from '@angular/core/testing';

import { BingNewsService } from './bing-news.service';

describe('BingNewsService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [BingNewsService]
    });
  });

  it('should ...', inject([BingNewsService], (service: BingNewsService) => {
    expect(service).toBeTruthy();
  }));
});
