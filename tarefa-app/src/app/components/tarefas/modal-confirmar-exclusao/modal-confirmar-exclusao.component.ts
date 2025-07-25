import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-modal-confirmar-exclusao',
  standalone: true,
  imports: [
    CommonModule
  ],
  templateUrl: './modal-confirmar-exclusao.component.html',
  styleUrl: './modal-confirmar-exclusao.component.css'
})
export class ModalConfirmarExclusaoComponent {
  @Input() show = false;
  @Output() confirm = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  onConfirm() {
    this.confirm.emit();
  }
  onCancel() {
    this.cancel.emit();
  }
}
