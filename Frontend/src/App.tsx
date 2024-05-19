import { ReactElement } from 'react';
import './App.css';
import { MainPage } from './views/MainPage';
import { createBrowserRouter, Params, redirect, RouterProvider } from 'react-router-dom';
import { Layout } from './views/Layout';
import { AuthPage } from './views/AuthPage';
import { getFullLink } from './utils/api/shortenLink';

export const App = (): ReactElement => {
  const router = createBrowserRouter([
    {
      path: '/',
      element: <Layout />,
      children: [
        {
          path: '/',
          element: <MainPage />,
        },
        {
          path: '/auth',
          element: <AuthPage />,
        },
      ],
    },
    {
      path: '/:token',
      loader: async ({ request }: { request: Request; params: Params }) => {
        const token = request.url.split('/')[request.url.split('/').length - 1];
        console.log(token);
        if (token.length !== 6) {
          return redirect('/');
        }

        const data = await getFullLink(token);
        if (data.status === 200) {
          window.location.href = 'http://' + data.data;
          return null;
        } else {
          return redirect('/');
        }
      },
    },
  ]);

  return <RouterProvider router={router} />;
};
