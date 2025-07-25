import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ITarefa } from '../../../interfaces/ITarefa';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import { TarefaService } from '../../../services/tarefa.service';
import { catchError } from 'rxjs';

@Component({
  selector: 'app-form-tarefas',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatIconModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    CommonModule
  ],
  templateUrl: './form-tarefas.component.html',
  styleUrl: './form-tarefas.component.css'
})
export class FormTarefasComponent {
  _tarefa: ITarefa | null = null;
  acao: 'Criar' | 'Editar' = 'Criar';
  tarefaForm: FormGroup;
  minDate: string;
  formInvalido: boolean = false;

  constructor(private fb: FormBuilder, private tarefaService: TarefaService) {
    this.minDate = this.getTomorrow();
    this.tarefaForm = this.fb.group({
          TITULO: ['', [Validators.required, Validators.maxLength(100), Validators.minLength(3)]],
          DESCRICAO: ['', [Validators.required, Validators.maxLength(200), Validators.minLength(3)]],
          DATA_LIMITE: [null, [Validators.required]],
          CONCLUIDA: [false, [Validators.required]]
        });
  }

  @Input() set tarefa(tarefa: ITarefa | null) {
    if (tarefa) {
      this.acao = 'Editar';
      this._tarefa = tarefa;
      this.setForm();
    }
  }

  @Output() cancelar = new EventEmitter<void>();
  @Output() salvarOuEditarTarefa = new EventEmitter<void>();

  get tarefa() {
    return this._tarefa;
  }

  getTomorrow(): string {
    const tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);
    const year = tomorrow.getFullYear();
    const month = String(tomorrow.getMonth() + 1).padStart(2, '0');
    const day = String(tomorrow.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  setForm() {
    let dataLimite = this.tarefa!.dataLimite.toString();
    if (dataLimite && typeof dataLimite === 'string') {
      dataLimite = dataLimite.substring(0, 10);
    }

    this.tarefaForm.patchValue({
      TITULO: this.tarefa!.titulo,
      DESCRICAO: this.tarefa!.descricao,
      DATA_LIMITE: dataLimite,
      CONCLUIDA: this.tarefa!.concluida
    });
  }

  onCancelar() {
    this.acao = 'Criar';
    this.tarefa = null;
    this.cancelar.emit();
  }

  onSubmit() {
    if (this.validarForm()) {
      const tarefa = this.montarTarefa();
      if (this.acao === 'Editar')
        this.editarTarefa(tarefa);
      else
        this.salvarTarefa(tarefa);

    } else {
      this.formInvalido = true;
    }
  }

  montarTarefa(): ITarefa{
    const tarefa: ITarefa = {
      id: this.tarefa ? this.tarefa.id : null,
      titulo: this.tarefaForm.get('TITULO')?.value,
      descricao: this.tarefaForm.get('DESCRICAO')?.value,
      dataLimite: new Date(this.tarefaForm.get('DATA_LIMITE')?.value),
      concluida: this.tarefaForm.get('CONCLUIDA')?.value
    };

    return tarefa;
  }

  validarForm(): boolean {
    let formValid = true;
    if (!this.tarefaForm.valid) {
      formValid = false;
    }

    return formValid;
  }

  editarTarefa(tarefa: ITarefa) {
    this.tarefaService.editarTarefa(tarefa)
      .pipe(
        catchError(error => {
          throw error;
        })
      ).subscribe((tarefaEditada: ITarefa) => {
        this.salvarOuEditarTarefa.emit();
      });
  }

  salvarTarefa(tarefa: ITarefa) {
    this.tarefaService.salvarTarefa(tarefa)
      .pipe(
        catchError(error => {
          console.error('Erro detalhado:', error.error.errors);
          throw error;
        })
      ).subscribe((tarefaSalva: ITarefa) => {
        this.salvarOuEditarTarefa.emit();
      });
  }
}
