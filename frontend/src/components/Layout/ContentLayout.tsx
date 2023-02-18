export const ContentLayout: React.FC<{
  children: React.ReactNode;
}> = ({ children }) => (
  <div className="w-full flex justify-center">
    <div className="flex flex-col gap-4 w-11/12 md:w-7/12 max-w-[900px] mt-12">
      {children}
    </div>
  </div>
)
