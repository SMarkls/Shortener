import { ReactElement, useEffect, useState } from 'react';
import { useSearchParams } from 'react-router-dom';
import { DropDown } from '../components/DropDown/DropDown';
import { Graphic } from '../components/Graphic/Graphic';
import { getStatistics } from '../utils/api/shortenLink';

type StatisticData = {
  statistics: { browser: string; time: string }[];
  countOfUniqueUsers: number;
};

export const LinkPage = (): ReactElement => {
  const searchParams = useSearchParams()[0];
  const token = searchParams.get('token');
  const fullUrl = searchParams.get('fullUrl');
  const id = searchParams.get('id');

  const [statistics, setStatistics] = useState<StatisticData>();

  useEffect(() => {
    const fetchStatistics = async () => {
      if (!id) {
        return (window.location.href = '/');
      }

      const data = await getStatistics(id);
      if (data.status !== 200) {
        return (window.location.href = '/');
      }

      setStatistics(data.data);
    };

    fetchStatistics();
  }, [id]);

  return (
    <>
      {token && fullUrl && <DropDown fullUrl={fullUrl} token={token} />}
      <div>Графики</div>
      {statistics && (
        <Graphic
          countOfUniqueUsers={statistics?.countOfUniqueUsers}
          statistics={statistics?.statistics.map((x) => {
            return { browser: x.browser, time: new Date(Date.parse(x.time)) };
          })}
        />
      )}
    </>
  );
};
