import { Action } from 'redux';

export interface ActionPayload<T> {
  readonly payload: T;
}

export interface ActionMeta<T> {
  readonly meta: T;
}

export interface ReceivedAt {
  receivedAt: number;
}

export interface HashMeta {
  readonly hash: string;
}

export interface ErrorPayload {
  readonly payload: Error;
}

export type SuccessAction<T> = Action<string> &
  ActionPayload<T> &
  ActionMeta<HashMeta & ReceivedAt>;

export type FailureAction = Action & ActionMeta<HashMeta> & ErrorPayload;
export type RequestAction = Action & ActionMeta<HashMeta>;