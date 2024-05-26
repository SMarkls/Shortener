import { ReactElement } from 'react';

export const InfoIcon = (props: React.SVGProps<SVGSVGElement>): ReactElement => {
  return (
    <div className="hoverable-icon">
      <svg width="29" height="29" viewBox="0 0 29 29" fill="none" xmlns="http://www.w3.org/2000/svg" {...props}>
        <g clipPath="url(#clip0_9_106)">
          <path
            d="M14.4577 27.7723C21.523 27.7723 27.2503 21.8966 27.2503 14.6484C27.2503 7.40021 21.523 1.52441 14.4577 1.52441C7.3925 1.52441 1.66504 7.40021 1.66504 14.6484C1.66504 21.8966 7.3925 27.7723 14.4577 27.7723Z"
            stroke="current"
            strokeWidth="2"
            strokeLinecap="round"
            strokeLinejoin="round"
          />
          <path d="M11.5055 20.7056H17.4098" stroke="current" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" />
          <path d="M14.4577 20.7056V13.6388H12.4896" stroke="current" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" />
          <path
            d="M14.4577 9.09593C14.186 9.09593 13.9657 8.86994 13.9657 8.59116C13.9657 8.31239 14.186 8.0864 14.4577 8.0864"
            stroke="current"
            strokeLinecap="round"
            strokeLinejoin="round"
          />
          <path
            d="M14.4578 9.09593C14.7295 9.09593 14.9498 8.86994 14.9498 8.59116C14.9498 8.31239 14.7295 8.0864 14.4578 8.0864"
            stroke="current"
            strokeWidth="2"
            strokeLinecap="round"
            strokeLinejoin="round"
          />
        </g>
        <defs>
          <clipPath id="clip0_9_106">
            <rect width="27.5534" height="28.267" fill="white" transform="translate(0.68103 0.514893)" />
          </clipPath>
        </defs>
      </svg>
    </div>
  );
};
