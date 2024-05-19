import { ReactNode } from 'react';
import style from './Header.module.css';

type HeaderProps = {
  children: ReactNode;
};

export const Header = (props: HeaderProps): ReactNode => {
  return (
    <>
      <h1 className={style.header}>{props.children}</h1>
    </>
  );
};
