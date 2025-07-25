import { Injectable } from '@angular/core';
import { BaseHttpService } from './base-http.service';
import { ITarefa } from '../interfaces/ITarefa';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TarefaService {

  private readonly url = '/tarefa';
  
  constructor(private baseHttp: BaseHttpService) { }

  public listarTarefas() : Observable<ITarefa[]> {
    return this.baseHttp.getClient<ITarefa[]>(`${this.url}`);
  }

  public salvarTarefa(tarefa: ITarefa) : Observable<ITarefa> {
    return this.baseHttp.postClient<ITarefa>(`${this.url}`, tarefa);
  }

  public editarTarefa(tarefa: ITarefa) : Observable<ITarefa> {
    return this.baseHttp.putClient<ITarefa>(`${this.url}`, tarefa);
  }

  public deletarTarefa(id: string) : Observable<ITarefa> {
    return this.baseHttp.deleteClient<ITarefa>(`${this.url}/` + id);
  }
}
