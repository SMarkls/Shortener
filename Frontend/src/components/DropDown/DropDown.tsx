import { ReactElement } from 'react';
import style from './DropDown.module.css';
import QRCode from 'react-qr-code';

type DropDownProps = {
  token: string;
  fullUrl: string;
};

export const DropDown = ({ token, fullUrl }: DropDownProps): ReactElement => {
  return (
    <div className={style.dropDown}>
      <div className={style.links}>
        <p>{window.location.host + '/' + token}</p>
        <p>{fullUrl}</p>
      </div>
      <div className={style.qr}>
        <QRCode value={window.location.href + token} style={{ width: '100%', height: '100%' }} bgColor="#d9d9d9" />
      </div>
    </div>
  );
};
