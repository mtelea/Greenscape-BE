import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LegumeDetailsComponent } from './legume-details.component';

describe('LegumeDetailsComponent', () => {
  let component: LegumeDetailsComponent;
  let fixture: ComponentFixture<LegumeDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LegumeDetailsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LegumeDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
