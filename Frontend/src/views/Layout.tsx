import { ReactElement } from 'react';
import { Outlet } from 'react-router-dom';
import { Navbar } from '../components/Navbar/Navbar';

export const Layout = (): ReactElement => {
  return (
    <>
      <Navbar />
      <Outlet />
    </>
  );
};
