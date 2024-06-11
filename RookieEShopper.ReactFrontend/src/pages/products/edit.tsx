import { UploadOutlined } from "@ant-design/icons";
import { Edit, getValueFromEvent, useForm, useSelect } from "@refinedev/antd";
import MDEditor from "@uiw/react-md-editor";
import { Button, Form, Input, InputNumber, Select, Upload, message } from "antd";

export const ProductEdit = () => {
  const { formProps, saveButtonProps, queryResult, formLoading } = useForm({
    meta: {
      headers: {
        "Content-Type": "application/json",
      },
      hasBody: false
    },
  });

  const productData = queryResult?.data?.data;

  const { selectProps: categorySelectProps } = useSelect({
    resource: "categories",    
    optionLabel: "name"
  });

  return (
    <Edit saveButtonProps={saveButtonProps} isLoading={formLoading}>
      <Form {...formProps} layout="vertical">
        <Form.Item
          label={"Product's Name"}
          name={["name"]}
          rules={[
            {
              required: true,
            },
          ]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          label={"Description"}
          name="description"
          rules={[
            {
              required: true,
            },
          ]}
        >
          <MDEditor data-color-mode="light" />
        </Form.Item>
        <Form.Item  
          label={"Category"}
          name={["categoryId"]}
          initialValue={productData?.category?.id}
          rules={[
            {
              required: true,
            },
          ]}
        >
          <Select                    
          {...categorySelectProps} />
        </Form.Item>
        <Form.Item
          label={"Price"}
          name={["price"]}
          rules={[
            {        
              type: "number",
              min: 1,     
              required: true,
            },
          ]}
        >
          <InputNumber 
            prefix="₫"
            size = "large"
            formatter={(value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')}
            parser={(value) => value?.replace(/\$\s?|(,*)/g, '') as unknown as number} />
        </Form.Item>
        <Form.Item
          label={"Number of product"}
          name={["numOfProduct"]}
          rules={[
            {        
              type: "number",
              min: 1,     
              required: true,
            },
          ]}
        >
          <InputNumber 
            size = "large"/>
        </Form.Item>
        <Form.Item
          label={"Main product image"}
          name={["productImage"]}
          valuePropName="productImage"
          getValueFromEvent={getValueFromEvent}
          noStyle          
        >
          <Upload
            name="file"
            action={`https://localhost:7265/api/Products/Media`}
            multiple
            listType="picture"
            headers={{
              Authorization: `Bearer ${localStorage.getItem("access_token")}`,
            }}
            beforeUpload={(file) => {const isPNG = file.type === 'image/png';
              if (!isPNG) {
                message.error(`${file.name} is not a png file`);
              }
              return isPNG || Upload.LIST_IGNORE}}
            defaultFileList={[{
              uid: `${productData?.mainImagePath}`,
              name: `${productData?.mainImagePath}`,
              status: 'done',
              url: `https://localhost:7265/ProductImages/${productData?.id}/${productData?.mainImagePath}`,
              percent: 100,              
            }]}
          >
            <Button icon={<UploadOutlined />}>Click to Upload</Button>
          </Upload>
        </Form.Item>
      </Form>
    </Edit>
  );
};
