import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowPropertyPhotosComponent } from './show-property-photos.component';

describe('ShowPropertyPhotosComponent', () => {
  let component: ShowPropertyPhotosComponent;
  let fixture: ComponentFixture<ShowPropertyPhotosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowPropertyPhotosComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowPropertyPhotosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
