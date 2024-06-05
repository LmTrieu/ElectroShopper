import { NumberField, Show, TextField } from "@refinedev/antd";
import { useShow } from "@refinedev/core";
import { Typography, Image, Carousel } from "antd";

const { Title } = Typography;

export const ProductShow = () => {
  const { queryResult } = useShow({
    resource: "products", 
  });
  const { data, isLoading } = queryResult;

  const product = data?.data;

  const mainImagePath =`https://localhost:7265/ProductImages/${product?.id}/${product?.mainImagePath}`;
  
  let imageGallery;
  if(product?.imageGallery.length > 0){
    imageGallery = product?.imageGallery.map((picture: string) => picture);
    //imageGallery.map((picture) => imageGallery.push(picture))
    //imageGallery = `https://localhost:7265/ProductImages/${product?.id}/${product?.imageGallery}`;
  }
  
  const imgs = (product?.imageGallery.length > 0)? 
    (imageGallery = product?.imageGallery.map((picture: string) => `https://localhost:7265/ProductImages/${product.id}/${picture}`)) : 
    [];

  return (
    <Show isLoading={isLoading}>
      <Title level={5}>{"ID"}</Title>
      <NumberField value={product?.id ?? ""} />
      <Title level={5}>{"Product's Name"}</Title>
      <TextField value={product?.name} />
      <Title level={5}>{"Description"}</Title>
      <TextField value={product?.description} />
      <Title level={5}>{"Price (VND)"}</Title>
      <NumberField value={product?.price} />
      <Title level={5}>{"Main Image"}</Title>
      <Image width={400} src= {mainImagePath}/>
      <Title level={5}>{"Number of product"}</Title>
      <NumberField value={product?.numOfProduct} />
      <Title level={5}>{"Image Gallery"}</Title>
      <Image.PreviewGroup>
        {imgs.map((imageUrl: string, index: number) => (
        <Image
          key={imageUrl + index}
          {...imgs.value}
          width={300}
          alt={`Image ${index + 1}`}
          src={imageUrl}
          />
        ))}
      </Image.PreviewGroup>
      <Title level={5}>{"Appliable Coupons"}</Title>
      <TextField value={product?.appliableCoupons} />
    </Show>
  );
};
