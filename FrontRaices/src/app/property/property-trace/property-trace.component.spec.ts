import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PropertyTraceComponent } from './property-trace.component';

describe('PropertyTraceComponent', () => {
  let component: PropertyTraceComponent;
  let fixture: ComponentFixture<PropertyTraceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PropertyTraceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PropertyTraceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
