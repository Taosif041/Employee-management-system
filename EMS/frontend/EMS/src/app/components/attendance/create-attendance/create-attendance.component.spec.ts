import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateAttendanceComponent } from './create-attendance.component';

describe('CreateAttendanceComponent', () => {
  let component: CreateAttendanceComponent;
  let fixture: ComponentFixture<CreateAttendanceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateAttendanceComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateAttendanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
