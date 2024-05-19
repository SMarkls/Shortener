import { ReactNode } from 'react';
import style from './Input.module.css';

type InputProps = {
  type: 'text' | 'password' | 'email';
  placeholder?: string;
  onChange?: (value: string) => void;
};

export const Input = ({ type, placeholder, onChange }: InputProps): ReactNode => {
  return (
    <>
      <input className={style.input} type={type} placeholder={placeholder} onChange={(event) => onChange && onChange(event.target.value)} />
    </>
  );
};
