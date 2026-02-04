import { CommonModule } from '@angular/common';
import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-photo-action-button',
  imports: [CommonModule],
  templateUrl: './photo-action-button.html',
  styleUrl: './photo-action-button.css',
})
export class PhotoActionButtonComponent {
  disabled = input<boolean>();
  selected = input<boolean>();
  clickEvent = output<Event>();

  onClick(event: Event) {
      this.clickEvent.emit(event);
    }
  }
