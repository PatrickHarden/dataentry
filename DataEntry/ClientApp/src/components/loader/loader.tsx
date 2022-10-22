import React, { FC, useState } from 'react';
import { css } from 'styled-components';
import { RingLoader, ScaleLoader, HashLoader } from 'react-spinners';

interface LoaderProps {
  isLoading?: boolean,
  color?: string,
  css?: any,
  size?: number,
  loaderType?: string,
  width?: number,
  height?: number,
  margin?: string
}

const Loader: FC<LoaderProps> = (props) => {
  const [loading, setLoading] = useState(true)

  // default to RingLoader
  return (
    <div className='sweet-loading'>
      {(props.loaderType) ? <></> :
        <ScaleLoader
          color={(props.color) ? props.color : '#006A4D'}
          loading={(props.isLoading) ? props.isLoading : loading}
          css={(props.css) ? props.css : override}
          height={(props.height) ? props.height: 30}
          width={(props.width) ? props.width: 5}
          margin={(props.margin) ? props.margin: '2px'}
        />
      }
      {(props.loaderType === 'RingLoader') &&
        <RingLoader
          sizeUnit={"px"}
          size={(props.size) ? props.size : 150}
          color={(props.color) ? props.color : '#006A4D'}
          loading={(props.isLoading) ? props.isLoading : loading}
          css={(props.css) ? props.css : override}
        />}
      {(props.loaderType === 'ScaleLoader') &&
        <ScaleLoader
          color={(props.color) ? props.color : '#006A4D'}
          loading={(props.isLoading) ? props.isLoading : loading}
          css={(props.css) ? props.css : override}
        />}
      {(props.loaderType === 'HashLoader') &&
        <HashLoader
          sizeUnit={"px"}
          size={(props.size) ? props.size : 150}
          color={(props.color) ? props.color : '#006A4D'}
          loading={(props.isLoading) ? props.isLoading : loading}
          css={(props.css) ? props.css : override}
        />}
    </div>
  )
}

const override = css`
    margin: 0 auto;
    text-align:center;
`;

export default Loader;