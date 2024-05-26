import { ReactNode } from 'react';
import style from './Button.module.css';

type ButtonProps = {
  children: ReactNode;
  onClick?: () => void;
};

export const Button = (props: ButtonProps): ReactNode => {
  return (
    <>
      <button className={style.btn} onClick={props.onClick}>
        {props.children}
      </button>
    </>
  );
};
