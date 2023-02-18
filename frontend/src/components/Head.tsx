import React from "react";
import { Helmet } from "react-helmet-async";

type HeadProps = {
  title?: string;
  description?: string;
};

export const Head = ({ title = '', description = '' }: HeadProps = {}) => {
  return (
    <Helmet
      title={title ? `${title} | Каталог КФУ` : undefined}
      defaultTitle="Каталог КФУ"
    >
      <meta name="description" content={description} />
    </Helmet>
  );
};
