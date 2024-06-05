import { ReactNode } from 'react';
import style from './ShortenLinkItem.module.css';
import { BinIconIcon } from '../Icons/BinIcon';
import { InfoIcon } from '../Icons/InfoIcon';
import { deleteShortenLink } from '../../utils/api/shortenLink';
import { Link } from 'react-router-dom';

type ShortenLinkItemProps = {
  token: string;
  fulllUrl: string;
  countRedirections: number;
  id: number;
  cb?: (id: number) => void;
};

export const ShortenLinkItem = ({ token, fulllUrl, countRedirections, id, cb }: ShortenLinkItemProps): ReactNode => {
  const onDeleteClicked = async () => {
    const response = await deleteShortenLink(id);
    if (response.status === 200 && cb) {
      cb(id);
    }
  };
  return (
    <div className={style.item}>
      <div>Токен</div>
      <div>Полная ссылка</div>
      <div>
        Кол-во
        <br />
        переходов
      </div>
      <div></div>
      <div>{token}</div>
      <div>{fulllUrl}</div>
      <div>{countRedirections}</div>
      <div className={style.icons}>
        <Link to={`/info?token=${token}&fullUrl=${fulllUrl}`}>
          <InfoIcon />
        </Link>
        <BinIconIcon onClick={onDeleteClicked} />
      </div>
    </div>
  );
};
